-- =========================================================================
-- ������ ���������� � ������� �������� � ���
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

-- ��������� ����������� �� ��� ���� �� ��� � ���������� ������
-- ����������� ��������
-- Id - ������������� �������������
-- CreateDate - ���� ���������� �����������
-- Number - ����� �������������
-- Year - ���
CREATE PROCEDURE [dbo].[GetCertificateByFioAndPassport]
	@LastName NVARCHAR(255) = null,				-- ������� ����������������
	@FirstName NVARCHAR(255) = null,				-- ��� ����������������
	@PatronymicName NVARCHAR(255) = null,			-- �������� ����������������
	@PassportSeria NVARCHAR(20) = null,		-- ����� ��������� ���������������� (��������)
	@PassportNumber NVARCHAR(20) = null,		-- ����� ��������� ���������������� (��������)	
	@CurrentCertificateNumber NVARCHAR(255)		-- ����� �������������, ��� ����� ��������� ��� �������
AS
BEGIN	
	declare @yearFrom int, @yearTo int
	select @yearFrom = 2008, @yearTo = Year(GetDate())
	
	SELECT  c.Id,
        c.CreateDate,
        c.Number,
        c.Year,
        marks,
        case when ed.[ExpireDate] is null then '�� �������'
             else case when isnull(certificate_deny.[Id], 0) <> 0
                       then '������������'
                       else case when getdate() <= ed.[ExpireDate]
                                 then '�������������'
                                 else '����� ����'
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


