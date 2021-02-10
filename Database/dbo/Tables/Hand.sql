CREATE TABLE [dbo].[Hand] (
    [hand_id]      INT IDENTITY (1, 1) NOT NULL,
    [user_id]      INT NOT NULL,
    [hand_type_id] INT NOT NULL,
    CONSTRAINT [PK_Hand] PRIMARY KEY CLUSTERED ([hand_id] ASC),
    CONSTRAINT [Hand_hand_type_id_fk] FOREIGN KEY ([hand_type_id]) REFERENCES [dbo].[hand_type] ([id]),
    CONSTRAINT [Hand_Users_Id_fk] FOREIGN KEY ([user_id]) REFERENCES [dbo].[Users] ([Id])
);

