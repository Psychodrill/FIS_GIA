-- отрубаем все коннекты
alter database DBNAME set offline WITH ROLLBACK IMMEDIATE
alter database DBNAME set online

ALTER DATABASE GVUZ SET RECOVERY SIMPLE;
go
DBCC SHRINKFILE ('gvuz_log', 10);
go
ALTER DATABASE GVUZ SET RECOVERY FULL;
go

-- какие индексы нужно перестроить
SELECT 'ALTER INDEX [' + ix.name + '] ON [' + s.name + '].[' + t.name + '] ' +
       CASE WHEN ps.avg_fragmentation_in_percent > 40 THEN 'REBUILD' ELSE 'REORGANIZE' END +
       CASE WHEN pc.partition_count > 1 THEN ' PARTITION = ' + cast(ps.partition_number as nvarchar(max)) ELSE '' END
FROM   sys.indexes AS ix INNER JOIN sys.tables t
           ON t.object_id = ix.object_id
       INNER JOIN sys.schemas s
           ON t.schema_id = s.schema_id
       INNER JOIN (SELECT object_id, index_id, avg_fragmentation_in_percent, partition_number
                   FROM sys.dm_db_index_physical_stats (DB_ID(), NULL, NULL, NULL, NULL)) ps
           ON t.object_id = ps.object_id AND ix.index_id = ps.index_id
       INNER JOIN (SELECT object_id, index_id, COUNT(DISTINCT partition_number) AS partition_count
                   FROM sys.partitions
                   GROUP BY object_id, index_id) pc
           ON t.object_id = pc.object_id AND ix.index_id = pc.index_id
WHERE  ps.avg_fragmentation_in_percent > 10 AND
       ix.name IS NOT NULL
GO	   
	 
-- ПЕРЕСТРОЙКА ИНДЕКСОВ	
SET NOCOUNT ON
GO

--Set the fillfactor
DECLARE @FillFactor TINYINT
SELECT @FillFactor=80

DECLARE @StartTime DATETIME
SELECT @StartTime=GETDATE()

if object_id('tempdb..#TablesToRebuildIndex') is not null
begin
drop table #TablesToRebuildIndex
end

DECLARE @NumTables VARCHAR(20)

SELECT
s.[Name] AS SchemaName,
t.[name] AS TableName,
SUM(p.rows) AS RowsInTable
INTO #TablesToRebuildIndex
FROM
sys.schemas s
LEFT JOIN sys.tables t
ON  s.schema_id = t.schema_id
LEFT JOIN sys.partitions p
ON  t.object_id = p.object_id
LEFT JOIN sys.allocation_units a
ON  p.partition_id = a.container_id
WHERE
p.index_id IN ( 0, 1 ) -- 0 heap table , 1 table with clustered index
AND p.rows IS NOT NULL
AND a.type = 1  -- row-data only , not LOB
GROUP BY
s.[Name],
t.[name]
SELECT @NumTables=@@ROWCOUNT

DECLARE RebuildIndex CURSOR FOR
SELECT
ROW_NUMBER() OVER (ORDER BY ttus.RowsInTable),
ttus.SchemaName,
ttus.TableName,
ttus.RowsInTable
FROM
#TablesToRebuildIndex AS ttus
ORDER BY
ttus.RowsInTable
OPEN RebuildIndex

DECLARE @TableNumber VARCHAR(20)
DECLARE @SchemaName NVARCHAR(128)
DECLARE @tableName NVARCHAR(128)
DECLARE @RowsInTable VARCHAR(20)
DECLARE @Statement NVARCHAR(300)
DECLARE @Status NVARCHAR(300)

FETCH NEXT FROM RebuildIndex INTO @TableNumber, @SchemaName, @tablename, @RowsInTable
WHILE ( @@FETCH_STATUS = 0 )
BEGIN
SET @Status='Table '+@TableNumber+' of '+@NumTables+': Rebuilding indexes on '+@SchemaName+'.'+@tablename + ' ('+@RowsInTable+' rows)'
RAISERROR (@Status, 0, 1) WITH NOWAIT  --RAISERROR used to immediately output status

SET @Statement = 'ALTER INDEX ALL ON ['+@SchemaName+'].['+@tablename +'] REBUILD WITH (FILLFACTOR = '+CONVERT(VARCHAR(3), @FillFactor)+' )'
EXEC sp_executesql @Statement

FETCH NEXT FROM RebuildIndex INTO @TableNumber, @SchemaName, @tablename, @RowsInTable
END

CLOSE RebuildIndex
DEALLOCATE RebuildIndex

drop table #TablesToRebuildIndex

Print 'Total Elapsed Time: '+CONVERT(VARCHAR(100), DATEDIFF(minute, @StartTime, GETDATE()))+' minutes'

GO	   

--- ИСПОЛЬЗОВАНИЕ ПАМЯТИ SQL
SELECT count(*)AS cached_pages_count 
    ,name ,index_id 
FROM sys.dm_os_buffer_descriptors AS bd 
    INNER JOIN 
    (
        SELECT object_name(object_id) AS name 
            ,index_id ,allocation_unit_id
        FROM sys.allocation_units AS au
            INNER JOIN sys.partitions AS p 
                ON au.container_id = p.hobt_id 
                    AND (au.type = 1 OR au.type = 3)
        UNION ALL
        SELECT object_name(object_id) AS name   
            ,index_id, allocation_unit_id
        FROM sys.allocation_units AS au
            INNER JOIN sys.partitions AS p 
                ON au.container_id = p.hobt_id 
                    AND au.type = 2
    ) AS obj 
        ON bd.allocation_unit_id = obj.allocation_unit_id
WHERE database_id = db_id()
GROUP BY name, index_id 
ORDER BY cached_pages_count DESC


---- СЖИМАЕМ БД
set nocount on
declare @idx table(
	objnname sysname,
	schname sysname,
	index_id int, 
	partition_number int,
	currentSize bigint,
	compSize bigint,
	ScurrentSize bigint,
	ScompSize bigint)

declare 
	@sql varchar(max), 
	@tn varchar(128), 
	@sn varchar(128), 
	@oid int, 
	@iid int
	
declare c cursor local fast_forward for
	select object_schema_name(object_id), object_name(object_id), t.[object_id]
	from sys.tables t with(nolock)
	where t.[type] = 'U'
	order by object_schema_name(object_id), object_name(object_id)
open c
while 1=1
begin
	fetch next from c into @sn, @tn, @oid
	if @@FETCH_STATUS <> 0 break
	raiserror('%s.%s', 10, 1, @sn, @tn) with nowait
	
	-- Получаем оценку эффективности сжатия индексов
	insert into @idx
	exec sp_estimate_data_compression_savings @sn, @tn, NULL, NULL, 'PAGE' ;
	
	declare ic cursor local fast_forward for
		select si.name, si.index_id
		from @idx i
		inner join sys.indexes si
			on si.[object_id] = @oid
			and si.index_id = i.index_id
		-- Не сжимаем то, что плохо сжимается
		where i.ScurrentSize > i.ScompSize*1.3
		-- Не сжимаем маленькие индексы и таблицы
		and i.currentSize > 10000 -- >10Mb
	open ic
	declare @index varchar(128)
	while 1=1
	begin
		fetch next from ic into @index, @iid
		if @@FETCH_STATUS <> 0 break 
		print @iid
		-- Если index_id > 0, то это индекс
		if @iid > 0 
			set @sql = 'alter index [' + @index + '] on [' + @sn + '].[' + @tn + '] rebuild with(data_compression=page)'
		-- Если index_id = 0, то это "куча"
		else 
			set @sql = 'alter table [' + @sn + '].[' + @tn + '] rebuild with(data_compression=page)'
		raiserror('%s', 10, 1, @sql) with nowait
		exec(@sql)
	end
	close ic
	deallocate ic 
	delete from @idx
end


-- ПОИСК НЕИСПОЛЬЗУЕМЫХ ИНДЕКСОВ С ВЫВОДОМ СТАТИСТИКИ
 select
      ind.Index_id,
      obj.Name as TableName,
      ind.Name as IndexName,
      ind.Type_Desc,
      indUsage.user_seeks,
      indUsage.user_scans,
      indUsage.user_lookups,
      indUsage.user_updates,
      indUsage.last_user_seek,
      indUsage.last_user_scan,
      'drop index [' + ind.name + '] ON [' + obj.name + ']' as DropCommand
from
      Sys.Indexes as ind JOIN Sys.Objects as obj on ind.object_id=obj.Object_ID
      LEFT JOIN  sys.dm_db_index_usage_stats indUsage
            ON
                  ind.object_id = indUsage.object_id
                  AND ind.Index_id=indUsage.Index_id
where
      ind.type_desc<>'HEAP' and obj.type<>'S'
      AND objectproperty(obj.object_id,'isusertable') = 1
      AND (isnull(indUsage.user_seeks,0)=0 AND isnull(indUsage.user_scans,0)=0 and isnull(indUsage.user_lookups,0)=0)
order by obj.name,ind.Name

-- GET UNUSED INDEXES THAT APPEAR IN THE INDEX USAGE STATS TABLE
DECLARE @MinimumPageCount int
SET @MinimumPageCount = 500

SELECT	Databases.name AS [Database],
	object_name(Indexes.object_id) AS [Table],
	Indexes.name AS [Index],
	PhysicalStats.page_count as [Page_Count],
	CONVERT(decimal(18,2), PhysicalStats.page_count * 8 / 1024.0) AS [Total Size (MB)],
	CONVERT(decimal(18,2), PhysicalStats.avg_fragmentation_in_percent) AS [Frag %],
	ParititionStats.row_count AS [Row Count],
	CONVERT(decimal(18,2), (PhysicalStats.page_count * 8.0 * 1024)
		/ ParititionStats.row_count) AS [Index Size/Row (Bytes)]
FROM sys.dm_db_index_usage_stats UsageStats
	INNER JOIN sys.indexes Indexes
		ON Indexes.index_id = UsageStats.index_id
			AND Indexes.object_id = UsageStats.object_id
	INNER JOIN SYS.databases Databases
		ON Databases.database_id = UsageStats.database_id
	INNER JOIN sys.dm_db_index_physical_stats (DB_ID(),NULL,NULL,NULL,NULL)
			AS PhysicalStats
		ON PhysicalStats.index_id = UsageStats.Index_id
			and PhysicalStats.object_id = UsageStats.object_id
	INNER JOIN SYS.dm_db_partition_stats ParititionStats
		ON ParititionStats.index_id = UsageStats.index_id
			and ParititionStats.object_id = UsageStats.object_id
WHERE UsageStats.user_scans = 0
	AND UsageStats.user_seeks = 0
	-- ignore indexes with less than a certain number of pages of memory
	AND PhysicalStats.page_count > @MinimumPageCount
	-- Exclude primary keys, which should not be removed
	AND Indexes.type_desc != 'CLUSTERED'
	and Databases.name = 'gvuz_test'
ORDER BY [Page_Count] DESC

WITH cte AS (
    SELECT 
    	'['+ c.name + '].[' + o.name + ']' AS TableName,
    	i.name AS IndexName,
    	i.index_id AS IndexID,   
    	user_seeks + user_scans + user_lookups AS Reads,
    	user_updates AS Writes,
    	(
    		SELECT SUM(p.rows) 
    		FROM sys.partitions p 
    		WHERE p.index_id = s.index_id 
    			AND s.object_id = p.object_id
    	) AS TotalRows,
    	CASE
    		WHEN s.user_updates < 1 THEN 100
    		ELSE 1.00 * (s.user_seeks + s.user_scans + s.user_lookups) 
    			/ s.user_updates
    	END AS ReadsPerWrite,
    	'DROP INDEX ' + QUOTENAME(i.name) 
    		+ ' ON ' + QUOTENAME(c.name) 
    		+ '.' + QUOTENAME(OBJECT_NAME(s.object_id)) 
    		AS 'DropSQL'
    FROM sys.dm_db_index_usage_stats s  
    INNER JOIN sys.indexes i ON i.index_id = s.index_id 
    	AND s.object_id = i.object_id   
    INNER JOIN sys.objects o on s.object_id = o.object_id
    INNER JOIN sys.schemas c on o.schema_id = c.schema_id
    WHERE OBJECTPROPERTY(s.object_id,'IsUserTable') = 1
    	AND s.database_id = DB_ID()   
    	AND i.type_desc = 'nonclustered'
    	AND i.is_primary_key = 0
    	AND i.is_unique_constraint = 0
    AND 
    (
    	SELECT SUM(p.rows) 
    	FROM sys.partitions p 
    	WHERE p.index_id = s.index_id 
    		AND s.object_id = p.object_id
    ) > 10000
)
SELECT * FROM cte


SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

/*кто кого*/
SELECT DB_NAME(pr1.dbid) AS 'DB'
      ,pr1.spid AS 'ID жертвы'
      ,RTRIM(pr1.loginame) AS 'Login жертвы'
      ,pr2.spid AS 'ID виновника'
      ,RTRIM(pr2.loginame) AS 'Login виновника'
      ,pr1.program_name AS 'программа жертвы'
      ,pr2.program_name AS 'программа виновника'
      ,txt.[text] AS 'Запрос виновника'
FROM   MASTER.dbo.sysprocesses pr1(NOLOCK)
       JOIN MASTER.dbo.sysprocesses pr2(NOLOCK)
            ON  (pr2.spid = pr1.blocked)
       OUTER APPLY sys.[dm_exec_sql_text](pr2.[sql_handle]) AS txt
WHERE  pr1.blocked <> 0

/* Кто что блокирует */
SELECT s.[nt_username]
      ,request_session_id
      ,tran_locks.[request_status]
      ,rd.[Description] + ' (' + tran_locks.resource_type + ' ' + tran_locks.request_mode + ')' [Object]
      ,txt_blocked.[text]
      ,COUNT(*) [COUNT]
FROM   sys.dm_tran_locks AS tran_locks WITH (NOLOCK)
       JOIN sys.sysprocesses AS s WITH (NOLOCK)
            ON  tran_locks.request_session_id = s.[spid]
       JOIN (
                SELECT 'KEY' AS sResource_type
                      ,p.[hobt_id] AS [id]
                      ,QUOTENAME(o.name) + '.' + QUOTENAME(i.name) AS [Description]
                FROM   sys.partitions p
                       JOIN sys.objects o
                            ON  p.object_id = o.object_id
                       JOIN sys.indexes i
                            ON  p.object_id = i.object_id
                            AND p.index_id = i.index_id
                UNION ALL
                SELECT 'RID' AS sResource_type
                      ,p.[hobt_id] AS [id]
                      ,QUOTENAME(o.name) + '.' + QUOTENAME(i.name) AS [Description]
                FROM   sys.partitions p
                       JOIN sys.objects o
                            ON  p.object_id = o.object_id
                       JOIN sys.indexes i
                            ON  p.object_id = i.object_id
                            AND p.index_id = i.index_id
                UNION ALL
                SELECT 'PAGE'
                      ,p.[hobt_id]
                      ,QUOTENAME(o.name) + '.' + QUOTENAME(i.name)
                FROM   sys.partitions p
                       JOIN sys.objects o
                            ON  p.object_id = o.object_id
                       JOIN sys.indexes i
                            ON  p.object_id = i.object_id
                            AND p.index_id = i.index_id
               
                UNION ALL
                SELECT 'OBJECT'
                      ,o.[object_id]
                      ,QUOTENAME(o.name)
                FROM   sys.objects o
            ) AS RD
            ON  RD.[sResource_type] = tran_locks.resource_type
            AND RD.[id] = tran_locks.resource_associated_entity_id
       OUTER APPLY sys.[dm_exec_sql_text](s.[sql_handle]) AS txt_Blocked
WHERE  (
           tran_locks.request_mode = 'X'
           AND tran_locks.resource_type = 'OBJECT'
       )
       OR  tran_locks.[request_status] = 'WAIT'
GROUP BY
       s.[nt_username]
      ,request_session_id
      ,tran_locks.[request_status]
      ,rd.[Description] + ' (' + tran_locks.resource_type + ' ' + tran_locks.request_mode + ')'
      ,txt_blocked.[text]
ORDER BY
       6 DESC
       
GO      

/* Чем занят сервер*/
SELECT s.[spid]
      ,s.[loginame]
      ,s.[open_tran]
      ,s.[blocked]
      ,s.[waittime]
      ,s.[cpu]
      ,s.[physical_io]
      ,s.[memusage]
       INTO #sysprocesses
FROM   sys.[sysprocesses] s

WAITFOR DELAY '00:00:05'

SELECT txt.[text]
      ,s.[spid]
      ,s.[loginame]
      ,s.[hostname]
      ,DB_NAME(s.[dbid]) [db_name]
      ,SUM(s.[waittime] -ts.[waittime]) [waittime]
      ,SUM(s.[cpu] -ts.[cpu]) [cpu]
      ,SUM(s.[physical_io] -ts.[physical_io]) [physical_io]
      ,s.[program_name]
FROM   sys.[sysprocesses] s
       JOIN #sysprocesses ts
            ON  s.[spid] = ts.[spid]
            AND s.[loginame] = ts.[loginame]
       OUTER APPLY sys.[dm_exec_sql_text](s.[sql_handle]) AS txt
WHERE  s.[cpu] -ts.[cpu]
       + s.[physical_io] -ts.[physical_io]
       > 500
       OR  (s.[waittime] -ts.[waittime]) > 3000
GROUP BY
       txt.[text]
      ,s.[spid]
      ,s.[loginame]
      ,s.[hostname]
      ,DB_NAME(s.[dbid])
      ,s.[program_name]
ORDER BY
       [physical_io] DESC
       
DROP TABLE #sysprocesses