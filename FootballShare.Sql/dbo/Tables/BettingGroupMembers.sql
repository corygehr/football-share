CREATE TABLE [dbo].[BettingGroupMembers] (
    [BettingGroupId] INT                NOT NULL,
    [SiteUserId]     UNIQUEIDENTIFIER   NOT NULL,
    [IsAdmin]        BIT                DEFAULT ((0)) NOT NULL,
    [WhenJoined]     DATETIMEOFFSET (7) NOT NULL,
    PRIMARY KEY CLUSTERED ([BettingGroupId] ASC, [SiteUserId] ASC),
    CONSTRAINT [FK_BettingGroupMembers_BettingGroups] FOREIGN KEY ([BettingGroupId]) REFERENCES [dbo].[BettingGroups] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_BettingGroupMembers_SiteUsers] FOREIGN KEY ([SiteUserId]) REFERENCES [dbo].[SiteUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

