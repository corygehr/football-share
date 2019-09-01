CREATE TABLE [dbo].[Teams] (
    [Id]        VARCHAR (64)   NOT NULL,
    [LeagueId]  VARCHAR (64)   NULL,
    [Name]      NVARCHAR (128) NOT NULL,
    [ShortName] NVARCHAR (32)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Teams_ToLeagues] FOREIGN KEY ([LeagueId]) REFERENCES [dbo].[Leagues] ([Id]) ON UPDATE CASCADE
);

