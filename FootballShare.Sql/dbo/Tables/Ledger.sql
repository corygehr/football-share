CREATE TABLE [dbo].[Ledger]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PoolId] INT NOT NULL, 
    [SiteUserId] UNIQUEIDENTIFIER NOT NULL, 
    [WagerId] INT NULL, 
    [StartingBalance] FLOAT NOT NULL, 
    [TransactionAmount] FLOAT NOT NULL, 
    [Description] VARCHAR(64) NOT NULL, 
    [NewBalance] NCHAR(10) NOT NULL, 
    [WhenCreated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    CONSTRAINT [FK_Ledger_ToPools] FOREIGN KEY ([PoolId]) REFERENCES [Pools]([Id]), 
    CONSTRAINT [FK_Ledger_ToSiteUsers] FOREIGN KEY ([SiteUserId]) REFERENCES [SiteUsers]([Id]), 
    CONSTRAINT [FK_Ledger_ToWagers] FOREIGN KEY ([WagerId]) REFERENCES [Wagers]([Id])
)
