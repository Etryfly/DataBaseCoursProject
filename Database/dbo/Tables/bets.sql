CREATE TABLE [dbo].[bets] (
    [Bet]     MONEY NOT NULL,
    [user_id] INT   NOT NULL,
    PRIMARY KEY CLUSTERED ([user_id] ASC),
    CONSTRAINT [bets_Users_Id_fk] FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users] ([Id])
);


GO
CREATE TRIGGER Bet_INSERT
ON bets
AFTER INSERT, UPDATE
AS
INSERT INTO Transactions (user_id, value)
SELECT inserted.user_id, inserted.Bet
FROM INSERTED