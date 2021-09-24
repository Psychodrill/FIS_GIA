-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (55, '055_2012_06_25_GetCertificateByFioAndPassport.sql')
-- =========================================================================

/****** Object:  StoredProcedure [dbo].[GetCertificateByFioAndPassport]    Script Date: 06/25/2012 17:55:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCertificateByFioAndPassport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetCertificateByFioAndPassport]
GO

/****** Object:  StoredProcedure [dbo].[GetCertificateByFioAndPassport]    Script Date: 06/25/2012 17:55:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Получение свидетельст за все года по ФИО и паспортным данным
-- Возвращаемы значения
-- Id - идентификатор свидетельства
-- CreateDate - Дата добавления сертификата
-- Number - номер свидетельства
-- Year - год
CREATE PROCEDURE [dbo].[GetCertificateByFioAndPassport]
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
             else case when isnull(certificate_deny.[Id], 0) <> 0
                       then 'Аннулировано'
                       else case when getdate() <= ed.[ExpireDate]
                                 then 'Действительно'
                                 else 'Истек срок'
                            end
                  end
        end as [Status]
FROM    dbo.CommonNationalExamCertificate c
        CROSS APPLY ( SELECT    ( SELECT    CAST(s.SubjectId AS VARCHAR(20))
                                            + '='
                                            + REPLACE(CAST(s.Mark AS VARCHAR(20)),
                                                      ',', '.') + ',' AS [text()]
                                  FROM      dbo.CommonNationalExamCertificateSubject s
                                  WHERE     s.CertificateId = c.Id
                                            AND s.Mark IS NOT NULL
                                FOR
                                  XML PATH(''),
                                      TYPE
                                ) marks
                    ) as marks
        LEFT JOIN dbo.ExpireDate ed ON ed.Year = c.Year
        LEFT OUTER JOIN CommonNationalExamcertificateDeny certificate_deny
        with ( nolock, fastfirstrow ) on certificate_deny.[Year] between @yearFrom and @yearTo
                                         and certificate_deny.certificateNumber = c.[Number]
WHERE    ( LastName = @LastName
            or @LastName is null
          )
          AND ( FirstName = @FirstName
                or @FirstName is null
              )
          AND ( PatronymicName = null
                or @PatronymicName is null
              )
          AND PassportNumber = @PassportNumber
          AND PassportSeria = @PassportSeria
          AND c.Number <> @CurrentCertificateNumber
END
GO


