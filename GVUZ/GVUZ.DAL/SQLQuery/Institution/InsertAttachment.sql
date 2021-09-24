-- добавление новой записи в Attachment
--declare @FileId uniqueidentifier
--declare @FileName varchar
--declare @DisplayName varchar
--declare @ContentType varchar
--declare @ContentLength varchar
--declare @Contentvarbinary(max)

insert into Attachment (FileID, Name, DisplayName, MimeType, ContentLength, Body)
values(@FileId, @FileName, @DisplayName, @ContentType, @ContentLength, @Content)

select SCOPE_IDENTITY()
