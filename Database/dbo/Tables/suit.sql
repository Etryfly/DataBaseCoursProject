CREATE TABLE [dbo].[suit] (
    [suit_id] INT          NOT NULL,
    [name]    VARCHAR (10) NOT NULL,
    CONSTRAINT [suit_pk] PRIMARY KEY NONCLUSTERED ([suit_id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [suit_suit_id_uindex]
    ON [dbo].[suit]([suit_id] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [suit_name_uindex]
    ON [dbo].[suit]([name] ASC);

