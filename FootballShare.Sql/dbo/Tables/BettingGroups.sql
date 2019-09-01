CREATE TABLE [dbo].[BettingGroups] (
    [Id]          INT                IDENTITY (1, 1) NOT NULL,
    [IsPublic]    BIT                DEFAULT ((0)) NOT NULL,
    [Description] VARCHAR (1024)     NULL,
    [Name]        VARCHAR (128)      NOT NULL,
    [WhenCreated] DATETIMEOFFSET (7) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

