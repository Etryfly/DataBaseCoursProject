CREATE TABLE [dbo].[hand_card] (
    [hand_card_id] INT IDENTITY (1, 1) NOT NULL,
    [hand_id]      INT NOT NULL,
    [card_id]      INT NOT NULL,
    CONSTRAINT [PK_hand_card] PRIMARY KEY CLUSTERED ([hand_card_id] ASC),
    CONSTRAINT [hand_card_Card_card_id_fk] FOREIGN KEY ([card_id]) REFERENCES [dbo].[Card] ([card_id]),
    CONSTRAINT [hand_card_Hand_hand_id_fk] FOREIGN KEY ([hand_id]) REFERENCES [dbo].[Hand] ([hand_id])
);

