CREATE TABLE [dbo].[Spreads] (
    [Id]             INT NOT NULL IDENTITY,
    [WeekEventId] INT NOT NULL, 
    [AwaySpread] FLOAT NULL,
    [HomeSpread] FLOAT NULL, 
    [WhenCreated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    [WhenUpdated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE())
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Spreads_ToWeekEvents] FOREIGN KEY ([WeekEventId]) REFERENCES [WeekEvents]([Id])
);

