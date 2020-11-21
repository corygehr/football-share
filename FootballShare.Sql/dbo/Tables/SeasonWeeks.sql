CREATE TABLE [dbo].[SeasonWeeks] (
    [Id]             VARCHAR (64)   NOT NULL,
    [SeasonId]       VARCHAR (64)   NOT NULL,
    [Sequence]       INT            NOT NULL,
    [Name]           NVARCHAR (128) NOT NULL,
    [StartDate]      DATE           NOT NULL,
    [EndDate]        DATE           NULL,
    [IsChampionship] BIT            DEFAULT ((0)) NOT NULL,
    [IsPlayoff]      BIT            DEFAULT ((0)) NOT NULL,
    [IsPreseason]    BIT            DEFAULT ((0)) NOT NULL,
    [WhenCreated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    [WhenUpdated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SeasonWeeks_ToSeason] FOREIGN KEY ([SeasonId]) REFERENCES [dbo].[Seasons] ([Id]) ON UPDATE CASCADE
);

