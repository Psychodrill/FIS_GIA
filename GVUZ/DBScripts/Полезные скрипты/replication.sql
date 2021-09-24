---------------------------------------------------------------------------------------------------
-- Ожидает окончание выполнения sql агентом указанного задания (job).
---------------------------------------------------------------------------------------------------
-- 
-- Пример вызова с использованием имени задания -- 
-- 
-- declare @job_name nvarchar(64) set @job_name = N'Job 0001'
-- exec msdb.dbo.sp_start_job         @job_name = @job_name 
-- exec msdb.dbo.sp_wait_job_activity @job_name = @job_name 
-- 
---------------------------------------------------------------------------------------------------
-- 
-- Пример вызова с использованием идентификатора задания -- 
-- 
-- declare @job_id nvarchar(64)   set @job_id = N'2E9ADA19-23E3-4C30-B7D6-5289E87060D6'
-- exec msdb.dbo.sp_start_job         @job_id = @job_id 
-- exec msdb.dbo.sp_wait_job_activity @job_id = @job_id 
-- 
---------------------------------------------------------------------------------------------------
--
-- Пример вызова нескольких заданий с использованием имени задания -- 
--
-- declare @job_name nvarchar(64) 
-- 
-- set @job_name = N'Job 0001'
-- exec msdb.dbo.sp_start_job         @job_name = @job_name 
-- exec msdb.dbo.sp_wait_job_activity @job_name = @job_name 
-- 
-- set @job_name = N'Job 0002'
-- exec msdb.dbo.sp_start_job         @job_name = @job_name 
-- exec msdb.dbo.sp_wait_job_activity @job_name = @job_name 
-- 
-- set @job_name = N'Job 0003'
-- exec msdb.dbo.sp_start_job         @job_name = @job_name 
-- exec msdb.dbo.sp_wait_job_activity @job_name = @job_name 
--
---------------------------------------------------------------------------------------------------

use [msdb]
go

set ansi_nulls on
go
set quoted_identifier off
go

if exists ( 
	select * from sys.objects 
	where object_id = object_id( N'[dbo].[sp_wait_job_activity]') and type in ( N'P', N'PC' ) 
)
begin
	drop procedure [dbo].[sp_wait_job_activity]
end
go

---------------------------------------------------------------------------------------------------
-- Ожидает окончание выполнения sql агентом указанного задания (job).
---------------------------------------------------------------------------------------------------
create procedure [dbo].[sp_wait_job_activity]
	@job_id   uniqueidentifier = NULL,  -- If provided should NOT also provide job_name
	@job_name sysname          = NULL   -- If provided should NOT also provide job_id
as
begin
	set nocount on

	select @job_name = ltrim(rtrim(@job_name))
	if (@job_name = N'') select @job_name = null

	if ( (@job_id is not null) or (@job_name is not null) )
	begin
		declare @retval int

		execute @retval = sp_verify_job_identifiers '@job_name',
                                                    '@job_id',
                                                     @job_name output,
                                                     @job_id   output
		if (@retval <> 0) return(1) 
	end
  
	while 0 = (
		select
			isnull( jh.run_status, 0 ) 
		from
			(msdb.dbo.sysjobactivity as ja with (nolock) 
			left join msdb.dbo.sysjobhistory as jh with (nolock) on ja.job_history_id = jh.instance_id)
			join msdb.dbo.sysjobs_view as j with (nolock) on ja.job_id = j.job_id
		where
			ja.job_id = ( select job_id from msdb.dbo.sysjobs with (nolock) where [name] = @job_name )
			and
			ja.session_id = ( select top(1) session_id from syssessions with (nolock) order by agent_start_date desc  )
	)
	begin
		waitfor delay '00:00:05'
	end

	return(0)
end

---------------------------------------------------------------------------------------------------
-- end of script -- 
---------------------------------------------------------------------------------------------------