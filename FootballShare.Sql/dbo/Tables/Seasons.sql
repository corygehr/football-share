CREATE TABLE [dbo].[Seasons] (
    [Id]        VARCHAR (64)   NOT NULL,
    [SportsLeagueId]  VARCHAR (64)   NOT NULL,
    [Name]      NVARCHAR (128) NOT NULL,
    [StartDate] DATE           NOT NULL,
    [EndDate]   DATETIMEOFFSET           NOT NULL,
    [WhenCreated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    [WhenUpdated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Seasons_ToLeagues] FOREIGN KEY ([SportsLeagueId]) REFERENCES [dbo].[Leagues] ([Id]) ON UPDATE CASCADE
);

