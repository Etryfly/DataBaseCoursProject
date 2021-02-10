CREATE TABLE [dbo].[GameStats] (
    [Loses]        INT   NOT NULL,
    [Wins]         INT   NOT NULL,
    [chips_earned] MONEY NOT NULL,
    [Id]           INT   NOT NULL,
    [chips_loosed] MONEY NOT NULL,
    [payed]        MONEY DEFAULT ((0)) NOT NULL,
    CONSTRAINT [GameStats_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [GameStats_Users_Id_fk] FOREIGN KEY ([Id]) REFERENCES [dbo].[Users] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [GameStats_Id_uindex]
    ON [dbo].[GameStats]([Id] ASC);

