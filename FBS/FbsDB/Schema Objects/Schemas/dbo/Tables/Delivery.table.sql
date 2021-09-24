CREATE TABLE [dbo].[Delivery] (
    [Id]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (255)  NOT NULL,
    [Message]      NVARCHAR (4000) NOT NULL,
    [TypeCode]     NVARCHAR (20)   NOT NULL,
    [CreateDate]   DATETIME        DEFAULT (getdate()) NOT NULL,
    [DeliveryDate] DATETIME        NOT NULL,
    [Status]       INT             DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF),
    FOREIGN KEY ([Status]) REFERENCES [dbo].[DeliveryStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

