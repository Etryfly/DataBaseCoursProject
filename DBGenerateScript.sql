create database DBKPDB
go
use DBKPDB

create table Money
(
    Id    int identity
        constraint PK_Money
            primary key,
    Chips money default 0 not null
)
go

create table Roles
(
    description varchar(30) not null,
    role_id     int         not null
        constraint Roles_pk
            primary key nonclustered
)
go

create table Users
(
    Login    nvarchar(max),
    Password nvarchar(max),
    Id       int           not null
        constraint Users_pk
            primary key nonclustered
        constraint Users_Money_Id_fk
            references Money,
    Role_id  int default 0 not null
        constraint Users_Roles_role_id_fk
            references Roles
)
go

create table GameStats
(
    Loses        int             not null,
    Wins         int             not null,
    chips_earned money           not null,
    Id           int             not null
        constraint GameStats_pk
            primary key nonclustered
        constraint GameStats_Users_Id_fk
            references Users,
    chips_loosed money           not null,
    payed        money default 0 not null
)
go

create unique index GameStats_Id_uindex
    on GameStats (Id)
go

create table Transactions
(
    id      int identity
        constraint Transactions_pk
            primary key nonclustered,
    user_id int
        constraint Transactions_Users_Id_fk
            references Users,
    value   int
)
go

create unique index Transactions_id_uindex
    on Transactions (id)
go

create unique index Users_Id_uindex
    on Users (Id)
go

create table bets
(
    Bet     money not null,
    user_id int   not null
        primary key
        constraint bets_Users_Id_fk
            references Users
)
go

CREATE TRIGGER Bet_INSERT
    ON bets
    AFTER INSERT, UPDATE
    AS
    INSERT INTO Transactions (user_id, value)
    SELECT inserted.user_id, inserted.Bet
    FROM INSERTED
go

create table hand_type
(
    id   int not null
        constraint hand_type_pk
            primary key nonclustered,
    type varchar(20)
)
go

create table Hand
(
    hand_id      int identity
        constraint PK_Hand
            primary key,
    user_id      int not null
        constraint Hand_Users_Id_fk
            references Users,
    hand_type_id int not null
        constraint Hand_hand_type_id_fk
            references hand_type
)
go

create table suit
(
    suit_id int         not null
        constraint suit_pk
            primary key nonclustered,
    name    varchar(10) not null
)
go

create table Card
(
    card_id int identity
        constraint PK_Card
            primary key,
    suit_id int not null
        constraint Card_suit_suit_id_fk
            references suit,
    Rank    int not null,
    Score   int not null
)
go

create table hand_card
(
    hand_card_id int identity
        constraint PK_hand_card
            primary key,
    hand_id      int not null
        constraint hand_card_Hand_hand_id_fk
            references Hand,
    card_id      int not null
        constraint hand_card_Card_card_id_fk
            references Card
)
go

create unique index suit_suit_id_uindex
    on suit (suit_id)
go

create unique index suit_name_uindex
    on suit (name)
go



INSERT [dbo].[hand_type] ([id], [type]) VALUES (1, N'User')
GO
INSERT [dbo].[hand_type] ([id], [type]) VALUES (2, N'Computer')


GO
INSERT [dbo].[Roles] ([description], [role_id]) VALUES (N'user', 0)
GO
INSERT [dbo].[Roles] ([description], [role_id]) VALUES (N'admin', 1)
GO
INSERT [dbo].[suit] ([suit_id], [name]) VALUES (1, N'Clubs')
GO
INSERT [dbo].[suit] ([suit_id], [name]) VALUES (2, N'Diamonds')
GO
INSERT [dbo].[suit] ([suit_id], [name]) VALUES (3, N'Hearts')
GO
INSERT [dbo].[suit] ([suit_id], [name]) VALUES (4, N'Spades')

go

INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (1, 1, 2, 2)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (2, 1, 3, 3)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (3, 1, 4, 4)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (4, 1, 5, 5)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (5, 1, 6, 6)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (6, 1, 7, 7)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (7, 1, 8, 8)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (8, 1, 9, 9)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (9, 1, 10, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (10, 1, 11, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (11, 1, 12, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (12, 1, 13, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (13, 1, 14, 11)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (14, 2, 2, 2)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (15, 2, 3, 3)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (16, 2, 4, 4)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (17, 2, 5, 5)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (18, 2, 6, 6)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (19, 2, 7, 7)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (20, 2, 8, 8)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (21, 2, 9, 9)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (22, 2, 10, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (23, 2, 11, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (24, 2, 12, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (25, 2, 13, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (26, 2, 14, 11)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (27, 3, 2, 2)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (28, 3, 3, 3)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (29, 3, 4, 4)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (30, 3, 5, 5)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (31, 3, 6, 6)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (32, 3, 7, 7)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (33, 3, 8, 8)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (34, 3, 9, 9)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (35, 3, 10, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (36, 3, 11, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (37, 3, 12, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (38, 3, 13, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (39, 3, 14, 11)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (40, 4, 2, 2)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (41, 4, 3, 3)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (42, 4, 4, 4)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (43, 4, 5, 5)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (44, 4, 6, 6)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (45, 4, 7, 7)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (46, 4, 8, 8)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (47, 4, 9, 9)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (48, 4, 10, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (49, 4, 11, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (50, 4, 12, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (51, 4, 13, 10)
GO
INSERT [dbo].[Card] ([card_id], [suit_id], [Rank], [Score]) VALUES (52, 4, 14, 11)
go

