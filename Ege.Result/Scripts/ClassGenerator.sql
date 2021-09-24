IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spWriteStringToFile')
drop procedure spWriteStringToFile
go

IF EXISTS (SELECT * FROM sys.objects WHERE type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) AND name = 'RemovePrefix')
drop function RemovePrefix
go

IF EXISTS (SELECT * FROM sys.objects WHERE type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) AND name = 'SingularizeRemovePrefix')
drop function SingularizeRemovePrefix
go

IF EXISTS (SELECT * FROM sys.objects WHERE type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ) AND name = 'singularize')
drop function singularize
go

if exists(select * from sys.assemblies where name = 'Singularize')
drop assembly Singularize
go

create assembly Singularize
	from 'C:\Projects\Result\Tools\Singularize.dll'
go

create function singularize
(
@String nvarchar(max)
) returns nvarchar(max)
as
external name Singularize.[Singularize.Singularizer].Singularize
--begin
--	return case
--		when right(@String, 1) = 's' then left(@String, len(@String) - 1)
--		else @String
--	end
--end
go

create function RemovePrefix(@string nvarchar(max)) returns nvarchar(max)
as
begin
	declare @prefixLength int = charindex('_', @string)
	declare @suffix nvarchar(max) = substring(@string, @prefixLength + 1, len(@string) - @prefixLength)

	return @suffix
end
go

create function SingularizeRemovePrefix(@string nvarchar(max)) returns nvarchar(max)
as
begin
	declare @prefixLength int = charindex('_', @string)
	declare @suffix nvarchar(max) = substring(@string, @prefixLength + 1, len(@string) - @prefixLength)

	return case
		when (exists (select 1 from [INFORMATION_SCHEMA].[TABLES] where TABLE_NAME = @suffix)) then substring(@string, 0, @prefixLength) + dbo.singularize(@suffix)
		else dbo.singularize(@suffix)
	end
end
go

create procedure spWriteStringToFile
 (
@String nvarchar(max),
@Path nvarchar(max),
@Filename nvarchar(max)
)
as
declare  
	@objFileSystem int
    ,@objTextStream int
	,@objErrorObject int
	,@strErrorMessage nvarchar(max)
	,@Command nvarchar(max)
	,@hr int
	,@fileAndPath nvarchar(max)
set nocount on

select @strErrorMessage='opening the File System Object'
execute @hr = sp_OACreate  'Scripting.FileSystemObject' , @objFileSystem out

select @FileAndPath=@path+'\'+@filename
if @hr=0 select @objErrorObject=@objFileSystem , @strErrorMessage='Creating file "'+@FileAndPath+'"'
if @hr=0 execute @hr = sp_OAMethod   @objFileSystem   , 'CreateTextFile'
	, @objTextStream OUT, @FileAndPath,2,True

if @hr=0 select @objErrorObject=@objTextStream, 
	@strErrorMessage='writing to the file "'+@FileAndPath+'"'
if @hr=0 execute @hr = sp_OAMethod  @objTextStream, 'Write', Null, @String

if @hr=0 select @objErrorObject=@objTextStream, @strErrorMessage='closing the file "'+@FileAndPath+'"'
if @hr=0 execute @hr = sp_OAMethod  @objTextStream, 'Close'

if @hr<>0
	begin
	declare 
		@Source nvarchar(max),
		@Description nvarchar(max),
		@Helpfile nvarchar(max),
		@HelpID int
	
	execute sp_OAGetErrorInfo  @objErrorObject, 
		@source output,@Description output,@Helpfile output,@HelpID output
	select @strErrorMessage='Error whilst '
			+coalesce(@strErrorMessage,'doing something')
			+', '+coalesce(@Description,'')
	raiserror (@strErrorMessage,16,1)
	end
execute  sp_OADestroy @objTextStream
execute sp_OADestroy @objTextStream
go

-- settings
declare @pathRoot nvarchar(50) = 'C:\Temp\Entities';
declare @rootNamespace nvarchar(100) = 'namespace Ege.Check.Dal.Entities';
declare @includeClassNameInNamespace bit = 0;
declare @createDirectoriesForEachClass bit = 0;
declare @includeNamespaces bit = 0;
declare @prefixCollectionNames bit = 0;

declare @tableName nvarchar(4000);
declare @singularTableName nvarchar(4000);
declare @tableDescription nvarchar(4000);
declare @prefix nvarchar(5) = '/// ';
declare @summaryBegin nvarchar(15) = @prefix + '<summary>'; 
declare @summaryEnd nvarchar(15) = @prefix + '</summary>';
declare @path nvarchar(4000);
declare @tab nvarchar(4) = '    ';
declare @tab2 nvarchar(8) = @tab + @tab;
declare @tab3 nvarchar(12) = @tab2 + @tab;
declare @endl nvarchar(2) = char(13) + char(10);
declare @usingsReplacer nvarchar(60) = '/**USINGS**/';
declare @hashSetsReplacer nvarchar(60) = '/**HASH_SETS**/';
declare @constructorReplacer nvarchar(60) = '/**CONSTRUCTOR**/';
declare @implementsReplacer nvarchar(60) = '/**IMPLEMENTS**/';
declare @commonUsings nvarchar(4000) = @usingsReplacer + @endl;

declare tablesCursor cursor 
  local static read_only forward_only
for 
select 
	table_name
	,isnull(cast(td.value as nvarchar(max)), '') as [Description]
from 
	information_schema.tables
	left join sys.extended_properties td on	td.major_id = object_id(table_name)
	and td.minor_id = 0
    and td.name = 'MS_Description'
where  
	table_type = 'BASE TABLE'
	--and Table_name = 'Users'
union
select 
	table_name
	,isnull(cast(td.value as nvarchar(max)), '') as [Description]
from 
	information_schema.views
	left join sys.extended_properties td on	td.major_id = object_id(table_name)
	and td.minor_id = 0
    and td.name = 'MS_Description'

open tablesCursor
	fetch next from tablesCursor into @tableName, @tableDescription
	while @@fetch_status = 0
begin 
	select @singularTableName = dbo.SingularizeRemovePrefix(@tableName)
	declare @namespaces nvarchar(max) = '';
	declare @implements nvarchar(max) = null;
	declare @hashSets nvarchar(max) = '';
	declare @isReferenceBook bit = 0;
    declare @result nvarchar(max) = 
	@rootNamespace + (case @includeClassNameInNamespace when 1 then '.' + @tableName else '' end) +
		@endl + '{' + 
		@commonUsings +
		@endl + @tab + @summaryBegin +
		@endl + @tab + @prefix + @tableDescription +
		@endl + @tab + @summaryEnd + 
		@endl + @tab + '[Table("' + @tableName + '")]' +
		@endl + @tab + 'public class ' + @singularTableName + @implementsReplacer +
		@endl + @tab + '{'
	if(@tableDescription LIKE '%' + 'справочник' + '%')
		set @isReferenceBook = 1
	set @result = @result + @constructorReplacer;

	declare @constructor nvarchar(max) = 
		@endl + @tab2 + 'public ' + @singularTableName + '()' +
		@endl + @tab2 + '{' +
		@hashSetsReplacer + 
		@endl + @tab2 + '}' + @endl;

	declare @hasCollections bit = 0;
	declare @namespacesTable Table(namespaceName nvarchar(max));
	declare @interfacesTable Table(interfaceName nvarchar(max));

	insert into @namespacesTable values ('System'), ('System.ComponentModel.DataAnnotations.Schema')
	declare @ColumnDescription nvarchar(max);
	declare @ColumnName nvarchar(max);
	declare @ColumnType nvarchar(max);
	declare @ColumnIsNullable nvarchar(1);
	declare @ReferencedTable nvarchar(max);

	declare simplePropsCursor cursor
		local static read_only forward_only
	for
		select 
			cast(sep.value as nvarchar(max)) as [Description],
			replace(col.name, ' ', '_') ColumnName,
			case typ.name 
				when 'bigint' then 'long'
				when 'binary' then 'byte[]'
				when 'bit' then 'bool'
				when 'char' then 'string'
				when 'date' then 'DateTime'
				when 'datetime' then 'DateTime'
				when 'datetime2' then 'DateTime'
				when 'datetimeoffset' then 'DateTimeOffset'
				when 'decimal' then 'decimal'
				when 'float' then 'float'
				when 'image' then 'byte[]'
				when 'int' then 'int'
				when 'money' then 'decimal'
				when 'nchar' then 'char'
				when 'ntext' then 'string'
				when 'numeric' then 'decimal'
				when 'nvarchar' then 'string'
				when 'real' then 'double'
				when 'smalldatetime' then 'DateTime'
				when 'smallint' then 'short'
				when 'smallmoney' then 'decimal'
				when 'text' then 'string'
				when 'time' then 'TimeSpan'
				when 'timestamp' then 'DateTime'
				when 'tinyint' then 'byte'
				when 'uniqueidentifier' then 'Guid'
				when 'varbinary' then 'byte[]'
				when 'varchar' then 'string'
				else 'UNKNOWN_' + typ.name
			end ColumnType,
			case col.is_nullable
				when 1 then '?'
				else ''
			end ColumnIsNullable,
			childtable.name ReferencedTable
		from sys.columns col
			join sys.types typ on
				col.system_type_id = typ.system_type_id and col.user_type_id = typ.user_type_id
			left join sys.extended_properties sep on sep.major_id = object_id(@tableName) and sep.minor_id = col.column_id and sep.name = 'MS_Description'
			left join sys.foreign_key_columns fk on fk.parent_column_id = col.column_id and fk.parent_object_id = col.object_id
			left join sys.columns child on child.column_id = fk.referenced_column_id and child.object_id = fk.referenced_object_id
			left join sys.tables childtable on childtable.object_id = child.object_id
		where 
			col.object_id = object_id(@tableName)
		order by col.column_id;
	declare @cursorCounter int = 0;
	open simplePropsCursor
			fetch next from simplePropsCursor into @ColumnDescription, @ColumnName, @ColumnType, @ColumnIsNullable, @ReferencedTable
			while @@fetch_status = 0
	begin
		if(@ColumnName = 'Id')
		begin
			if(@ColumnType = 'Guid')
				insert into @interfacesTable values ('IHasGuidId')
			else if(@ColumnType = 'int')
				insert into @interfacesTable values ('IHasIntId')
		end

		if(@ColumnName = 'LastModified' and @ColumnType = 'DateTime' and @ColumnIsNullable = '')
			insert into @interfacesTable values ('IHasLastModified')

		if(@ColumnName = 'CreateDate' and @ColumnType = 'DateTime' and @ColumnIsNullable = '')
			insert into @interfacesTable values ('IHasCreateDate')

		if(@ColumnName = 'CreatorId' and @ColumnType = 'Guid' and @ColumnIsNullable = '')
			insert into @interfacesTable values ('IHasCreator')

		if(@ColumnName = 'ModifierId' and @ColumnType = 'Guid' and @ColumnIsNullable = '')
			insert into @interfacesTable values ('IHasModifier')

		set @result = @result + 
			 @endl + @tab2 + @summaryBegin +
			@endl + @tab2 + @prefix + replace(isnull(@ColumnDescription, ''), @endl, @endl + @tab2 + @prefix) +
			@endl + @tab2 + @summaryEnd	 +
		@endl + @tab2 + 'public ' + @ColumnType + (
			case @ColumnType 
				when 'string' then ''
				when 'byte[]' then ''
				else @ColumnIsNullable 
			end) + ' ' + @ColumnName + ' { get; set; }' +
		case 
			when @ReferencedTable is not null then
				@endl +
				@endl + @tab2 + '[ForeignKey("' + @ColumnName + '")]' +
				@endl + @tab2 + 'public ' + dbo.SingularizeRemovePrefix(@ReferencedTable) + ' '  + 
					case substring(@ColumnName, 0, len(@ColumnName) - 1)
						when '' then dbo.SingularizeRemovePrefix(@ReferencedTable)
						else substring(@ColumnName, 0, len(@ColumnName) - 1)
					end
					 +
						' { get; set; }'
			else
				''
		end +
		@endl
		if((@ReferencedTable is not null) and @includeNamespaces = 1)
			insert into @namespacesTable (namespaceName) values (@ReferencedTable);

		fetch next from simplePropsCursor into  @ColumnDescription, @ColumnName, @ColumnType, @ColumnIsNullable, @ReferencedTable
	end
	CLOSE simplePropsCursor
	DEALLOCATE simplePropsCursor

	declare @InverseProperty nvarchar(max);
	declare @SingularParentTableName nvarchar(max);
	declare @ParentTableName nvarchar(max);

	declare collectionCursor cursor 
	  local static read_only forward_only
	for 
	select substring(ParentColumnName, 0, len(ParentColumnName) - 1), SingularParentTableName, ParentTableName
	from (
	select
		parenttable.name ParentTableName,
		dbo.SingularizeRemovePrefix(parenttable.name) SingularParentTableName,
		replace(parent.name, ' ', '_') ParentColumnName,
		col.name ColumnName,
		col.column_id column_id
	from
		sys.tables tbl
		join sys.columns col on tbl.object_id = col.object_id
		join sys.foreign_key_columns fk on fk.referenced_column_id = col.column_id and fk.referenced_object_id = col.object_id
		join sys.columns parent on parent.object_id = fk.parent_object_id and parent.column_id = fk.parent_column_id
		join sys.tables parenttable on parenttable.object_id = fk.parent_object_id
	where
		tbl.object_id = object_id(@tableName)
	) t order by column_id

	open collectionCursor
		fetch next from collectionCursor into @InverseProperty, @SingularParentTableName, @ParentTableName
		while @@fetch_status = 0
	begin 
	declare @collectionName nvarchar(450) = case when @prefixCollectionNames = 1 then @InverseProperty + @ParentTableName else dbo.RemovePrefix(@ParentTableName) end
	set  @result = @result + 
			@endl + @tab2 + '[InverseProperty("' + @InverseProperty + '")]' +
			@endl + @tab2 + 'public ICollection<' + @SingularParentTableName + '> ' + @collectionName + ' { get; set; }' +
			@endl;
	if (@includeNamespaces = 1) 
	begin
		insert into @namespacesTable values (@ParentTableName);
	end
	set @hasCollections = 1;
	set @hashSets = @hashSets + @endl + @tab3 + @collectionName + ' = new HashSet<' + @SingularParentTableName + '>();'
	fetch next from collectionCursor into @InverseProperty, @SingularParentTableName, @ParentTableName
	end
	CLOSE collectionCursor
	DEALLOCATE collectionCursor

	set @result = @result + @tab + '}' 

	set @result = @result
				+ @endl + '}'
				+ @endl;

	if(@hasCollections = 1)
		begin
			insert into @namespacesTable values ('System.Collections.Generic');
			select @result = Replace(@result, @constructorReplacer, @constructor);
		end
	else
		select @result = Replace(@result, @constructorReplacer, '');
	
	if(@isReferenceBook = 1)
		begin
			delete from @interfacesTable;
			insert into @interfacesTable values ('IReferenceBook');
		end

	SELECT
		@implements = CASE
			WHEN @implements IS NULL
			THEN it.interfaceName
			ELSE @implements + ', ' + it.interfaceName
		END
	FROM
		(select distinct interfaceName from @interfacesTable) as it

	if(Exists(select 1 from @interfacesTable))
	begin
		select @result = Replace(@result, @implementsReplacer, ': ' + @implements)
		insert into @namespacesTable values ('Interfaces')
	end
	else
		select @result = Replace(@result, @implementsReplacer, '')
		

	select @namespaces = @namespaces + @endl + @tab + 'using ' + res.nn + ';' from (select distinct namespaceName as nn from @namespacesTable)  as res order by nn
	
	select @result = Replace(@result, @usingsReplacer, @namespaces);
	select @result = Replace(@result, @hashSetsReplacer, @hashSets);
	
	set @path = @pathRoot + (case @createDirectoriesForEachClass when 1 then '\' + @tableName else '' end);
	exec master.dbo.xp_create_subdir @path;
	set @tableName = @singularTableName + '.cs';
	exec spWriteStringToFile @result, @path, @tableName;
	delete from @namespacesTable;
	delete from @interfacesTable;
	fetch next from tablesCursor into @tableName, @tableDescription
end
close tablesCursor
deallocate tablesCursor
