CREATE TABLE [dbo].[DeliveryLog] (
    [DeliveryId]       BIGINT          NULL,
    [ReciverEMail]     NVARCHAR (255)  NOT NULL,
    [Success]          BIT             NOT NULL,
    [EventDate]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [ErrorDescription] NVARCHAR (1000) DEFAULT (NULL) NULL,
    FOREIGN KEY ([DeliveryId]) REFERENCES [dbo].[Delivery] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

