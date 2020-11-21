CREATE TABLE [dbo].[WeekEvents] (
    [Id]           INT                NOT NULL,
    [AwayScore]    INT                NULL,
    [AwayTeamId]   VARCHAR (64)       NULL,
    [HomeScore]    INT                NULL,
    [HomeTeamId]   VARCHAR (64)       NULL,
    [Overtime]     INT                NULL,
    [Postponed]    BIT                DEFAULT ((0)) NOT NULL,
    [Completed] BIT NOT NULL DEFAULT 0,
    [SeasonWeekId] VARCHAR (64)       NOT NULL,
    [Time]         DATETIMEOFFSET (7) NULL
    PRIMARY KEY CLUSTERED ([Id] ASC) ,
    [WhenCreated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    [WhenUpdated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    CONSTRAINT [FK_WeekEvents_ToAwayTeams] FOREIGN KEY ([AwayTeamId]) REFERENCES [dbo].[Teams] ([Id]),
    CONSTRAINT [FK_WeekEvents_ToHomeTeams] FOREIGN KEY ([HomeTeamId]) REFERENCES [dbo].[Teams] ([Id]),
    CONSTRAINT [FK_WeekEvents_ToSeasonWeeks] FOREIGN KEY ([SeasonWeekId]) REFERENCES [dbo].[SeasonWeeks] ([Id])
);

