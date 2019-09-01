CREATE TABLE [dbo].[SiteUserLoginProviders] (
    [ExternalLoginId]     UNIQUEIDENTIFIER   NOT NULL,
    [UserId]              UNIQUEIDENTIFIER   NOT NULL,
    [LoginProvider]       VARCHAR (512)      NOT NULL,
    [ProviderKey]         VARCHAR (MAX)      NULL,
    [ProviderDisplayName] VARCHAR (512)      NOT NULL,
    [WhenRegistered]      DATETIMEOFFSET (7) NOT NULL,
    PRIMARY KEY CLUSTERED ([ExternalLoginId] ASC),
    CONSTRAINT [FK_SiteUserLoginProviders_SiteUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[SiteUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

