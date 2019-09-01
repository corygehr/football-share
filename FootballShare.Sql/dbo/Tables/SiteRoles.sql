﻿CREATE TABLE [dbo].[SiteRoles] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (256) NOT NULL,
    [NormalizedName] NVARCHAR (256) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_SiteRoles_NormalizedName]
    ON [dbo].[SiteRoles]([NormalizedName] ASC);

