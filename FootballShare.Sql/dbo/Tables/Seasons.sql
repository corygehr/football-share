CREATE TABLE [dbo].[Seasons] (
    [Id]        VARCHAR (64)   NOT NULL,
    [EndDate]   DATE           NOT NULL,
    [LeagueId]  VARCHAR (64)   NOT NULL,
    [Name]      NVARCHAR (128) NOT NULL,
    [StartDate] DATE           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Seasons_ToLeagues] FOREIGN KEY ([LeagueId]) REFERENCES [dbo].[Leagues] ([Id]) ON UPDATE CASCADE
);

