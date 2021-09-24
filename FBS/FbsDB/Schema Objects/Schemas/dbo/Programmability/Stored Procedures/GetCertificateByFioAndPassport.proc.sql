-- Получение свидетельст за все года по ФИО и паспортным данным
-- Возвращаемы значения
-- Id - идентификатор свидетельства
-- CreateDate - Дата добавления сертификата
-- Number - номер свидетельства
-- Year - год
alter PROCEDURE [dbo].[GetCertificateByFioAndPassport]
	@LastName NVARCHAR(255) = null,				-- фамилия сертифицируемого
	@FirstName NVARCHAR(255) = null,				-- имя сертифицируемого
	@PatronymicName NVARCHAR(255) = null,			-- отчетсво сертифицируемого
	@PassportSeria NVARCHAR(20) = null,		-- серия документа сертифицируемого (паспорта)
	@PassportNumber NVARCHAR(20) = null,		-- номер документа сертифицируемого (паспорта)	
	@CurrentCertificateNumber NVARCHAR(255)		-- номер свидетельства, его нужно исключить при выборке
AS
BEGIN	
	declare @yearFrom int, @yearTo int
	select @yearFrom = 2008, @yearTo = Year(GetDate())
	
	SELECT  c.Id,
        c.CreateDate,
        c.Number,
        c.Year,
        marks,
        case when ed.[ExpireDate] is null then 'Не найдено'
             else case when certificate_deny.CertificateFK is not null
                       then 'Аннулировано'
                       else case when getdate() <= ed.[ExpireDate]
                                 then 'Действительно'
                                 else 'Истек срок'
                            end
                  end
        end as [Status]
FROM    (
		 SELECT b.LicenseNumber AS Number, a.Surname AS LastName, a.Name AS FirstName, a.SecondName AS PatronymicName, b.CertificateID AS id, b.UseYear AS Year, 
                a.DocumentSeries AS PassportSeria, a.DocumentNumber AS PassportNumber, b.REGION AS RegionId, b.TypographicNumber, a.ParticipantID AS ParticipantsID, 
                REPLACE(REPLACE(LTRIM(RTRIM(a.Name)) + LTRIM(RTRIM(a.Surname)) + LTRIM(RTRIM(a.SecondName)), 'ё', 'е'), ' ', '') AS FIO, 
                a.ParticipantID, b.CreateDate
		 FROM rbd.Participants AS a with(nolock)
			INNER JOIN prn.Certificates AS b with(nolock) ON b.ParticipantFK = a.ParticipantID and b.UseYear=a.UseYear
		 where a.UseYear between @yearFrom and @yearTo and ( Surname = @LastName
            or @LastName is null
          )
          AND ( Name = @FirstName
                or @FirstName is null
              )
          AND ( SecondName = null
                or @PatronymicName is null
              )
          AND DocumentNumber = @PassportNumber
          AND DocumentSeries = @PassportSeria
          AND LicenseNumber <> @CurrentCertificateNumber			
		) c
        CROSS APPLY ( SELECT    ( SELECT    CAST(s.SubjectCode AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      [prn].[CertificatesMarks] s with(nolock)
                                  WHERE     s.CertificateFK = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
                    ) as marks
        LEFT JOIN dbo.ExpireDate ed ON ed.Year = c.Year
        LEFT OUTER JOIN prn.CancelledCertificates certificate_deny
        with ( nolock, fastfirstrow ) on certificate_deny.UseYear=c.Year
                                         and certificate_deny.CertificateFK=c.id
END