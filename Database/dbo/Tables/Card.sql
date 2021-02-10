CREATE TABLE [dbo].[Card] (
    [card_id] INT IDENTITY (1, 1) NOT NULL,
    [suit_id] INT NOT NULL,
    [Rank]    INT NOT NULL,
    [Score]   INT NOT NULL,
    CONSTRAINT [PK_Card] PRIMARY KEY CLUSTERED ([card_id] ASC),
    CONSTRAINT [Card_suit_suit_id_fk] FOREIGN KEY ([suit_id]) REFERENCES [dbo].[suit] ([suit_id])
);

