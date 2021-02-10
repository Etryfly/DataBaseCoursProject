CREATE TABLE [dbo].[Transactions] (
    [id]      INT IDENTITY (1, 1) NOT NULL,
    [user_id] INT NULL,
    [value]   INT NULL,
    CONSTRAINT [Transactions_pk] PRIMARY KEY NONCLUSTERED ([id] ASC),
    CONSTRAINT [Transactions_Users_Id_fk] FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Transactions_id_uindex]
    ON [dbo].[Transactions]([id] ASC);

