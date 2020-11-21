CREATE TABLE [dbo].[Teams] (
    [Id]        VARCHAR (64)   NOT NULL,
    [SportsLeagueId]  VARCHAR (64)   NULL,
    [Name]      NVARCHAR (128) NOT NULL,
    [ShortName] NVARCHAR (32)  NULL,
    [Abbreviation] VARCHAR(10) NULL, 
    [WhenCreated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    [WhenUpdated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Teams_ToLeagues] FOREIGN KEY ([SportsLeagueId]) REFERENCES [dbo].[Leagues] ([Id]) ON UPDATE CASCADE
);

