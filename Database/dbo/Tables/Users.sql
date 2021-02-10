CREATE TABLE [dbo].[Users] (
    [Login]    NVARCHAR (MAX) NULL,
    [Password] NVARCHAR (MAX) NULL,
    [Id]       INT            NOT NULL,
    [Role_id]  INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [Users_pk] PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [Users_Money_Id_fk] FOREIGN KEY ([Id]) REFERENCES [dbo].[Money] ([Id]),
    CONSTRAINT [Users_Roles_role_id_fk] FOREIGN KEY ([Role_id]) REFERENCES [dbo].[Roles] ([role_id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Users_Id_uindex]
    ON [dbo].[Users]([Id] ASC);

