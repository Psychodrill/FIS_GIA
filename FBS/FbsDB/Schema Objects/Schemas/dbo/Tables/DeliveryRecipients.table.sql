CREATE TABLE [dbo].[DeliveryRecipients] (
    [RecipientCode] INT    NULL,
    [DeliveryId]    BIGINT NULL,
    FOREIGN KEY ([DeliveryId]) REFERENCES [dbo].[Delivery] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

