CREATE TABLE [dbo].[SiteUsers] (
    [Id]                 UNIQUEIDENTIFIER   NOT NULL,
    [UserName]           NVARCHAR (128)     NOT NULL,
    [NormalizedUserName] NVARCHAR (128)     NOT NULL,
    [FullName]           NVARCHAR (128)     NULL,
    [DisplayName]        NVARCHAR (128)     NULL,
    [Email]              NVARCHAR (128)     NULL,
    [NormalizedEmail]    NVARCHAR (128)     NULL,
    [EmailConfirmed]     BIT                NOT NULL,
    [WhenRegistered]     DATETIMEOFFSET (7) NOT NULL,
    [WhenUpdated]        DATETIMEOFFSET (7) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_SiteUsers_NormalizedUserName]
    ON [dbo].[SiteUsers]([NormalizedUserName] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_SiteUsers_NormalizedEmail]
    ON [dbo].[SiteUsers]([NormalizedEmail] ASC);

