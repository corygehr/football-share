CREATE TABLE [dbo].[Pools] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [SeasonId] VARCHAR(64) NOT NULL, 
    [Name] VARCHAR(128) NOT NULL, 
    [IsPublic] BIT NOT NULL DEFAULT 0, 
    [StartingBalance] FLOAT NOT NULL, 
    [WagersPerWeek] INT NOT NULL, 
    [WhenCreated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    [WhenUpdated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    CONSTRAINT [PK_Pools] PRIMARY KEY ([Id])
);

