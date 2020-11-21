CREATE TABLE [dbo].[SiteUserRoles] (
    [SiteUserId] UNIQUEIDENTIFIER NOT NULL,
    [SiteRoleId] INT              NOT NULL,
    [WhenCreated] DATETIMEOFFSET NOT NULL DEFAULT (GETDATE()), 
    PRIMARY KEY CLUSTERED ([SiteUserId] ASC, [SiteRoleId] ASC),
    CONSTRAINT [FK_SiteRoles_RoleId] FOREIGN KEY ([SiteRoleId]) REFERENCES [dbo].[SiteRoles] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_SiteUsers_UserId] FOREIGN KEY ([SiteUserId]) REFERENCES [dbo].[SiteUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

