CREATE TABLE [dbo].[Roles] (
    [description] VARCHAR (30) NOT NULL,
    [role_id]     INT          NOT NULL,
    CONSTRAINT [Roles_pk] PRIMARY KEY NONCLUSTERED ([role_id] ASC)
);

