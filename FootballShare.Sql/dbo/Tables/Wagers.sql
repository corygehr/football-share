CREATE TABLE [dbo].[Wagers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PoolId] INT NOT NULL, 
    [SiteUserId] UNIQUEIDENTIFIER NOT NULL, 
    [WeekEventId] INT NOT NULL, 
    [SelectedTeamId] VARCHAR(64) NOT NULL, 
    [Amount] FLOAT NOT NULL, 
    [SelectedTeamSpread] FLOAT NOT NULL, 
    [Result] NCHAR(10) NULL, 
    [WhenCreated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    CONSTRAINT [FK_Wagers_ToPools] FOREIGN KEY ([PoolId]) REFERENCES [Pools]([Id])
)
