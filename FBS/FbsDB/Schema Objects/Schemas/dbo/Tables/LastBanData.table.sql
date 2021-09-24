CREATE TABLE [dbo].[BanData]
(
	AccountId bigint not null,
	WrongCheckCount int not null default(0),
	SuccessCheckCount int not null default(0),
	CheckDate datetime not null default(getdate())
)

GO

ALTER TABLE [dbo].[BanData]
    ADD CONSTRAINT [PK_BanData] PRIMARY KEY CLUSTERED (CheckDate ASC, [AccountId] ASC) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


