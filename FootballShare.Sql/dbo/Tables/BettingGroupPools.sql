CREATE TABLE [dbo].[BettingGroupPools] (
    [Id]             INT          IDENTITY (1, 1) NOT NULL,
    [BettingGroupId] INT          NOT NULL,
    [SeasonId]       VARCHAR (64) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BettingGroupPools_BettingGroups] FOREIGN KEY ([BettingGroupId]) REFERENCES [dbo].[BettingGroups] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_BettingGroupPools_Seasons] FOREIGN KEY ([SeasonId]) REFERENCES [dbo].[Seasons] ([Id]) ON UPDATE CASCADE
);

