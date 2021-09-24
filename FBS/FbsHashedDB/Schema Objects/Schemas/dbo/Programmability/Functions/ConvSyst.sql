create function ConvSyst(@num int, @syst int)
RETURNS varchar(99)
AS
BEGIN
	declare @r varchar(99)
	set @r=''

	if @syst>1 
		while @num>0 
			select @r=char(case when @num%@syst < 10
								then @num%@syst + ascii('0') 
								else @num%@syst + ascii('A')-10 
						   end)+','+@r,@num=@num / @syst
	return case when @r='' then '0' else substring(@r,1,len(@r)-1) end
END
