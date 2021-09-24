-- лог проверок свидетельств ЕГЭ
CREATE TABLE [dbo].[CheckCommonNationalExamCertificateLog] (
    [Date]              DATETIME       NOT NULL, -- дата
    [AccountId]         BIGINT         NULL, -- id аккунта проверяющего, если null то значит проверяли с открытого контура
    [CertificateNumber] NVARCHAR (255) NOT NULL, -- номер свидетельства
    [IsOriginal]        BIT            NULL,	 -- что это ?
    [IsBatch]           BIT            NULL,	 -- пакетная проверка или интерактивная
    [IsExist]           BIT            NULL      -- существует ли запрашиваемое свидетельство
);

