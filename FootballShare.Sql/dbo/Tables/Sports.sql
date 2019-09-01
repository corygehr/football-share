CREATE TABLE [dbo].[Sports] (
    [Id]        VARCHAR (64)   NOT NULL,
    [Name]      NVARCHAR (128) NOT NULL,
    [ShortName] NVARCHAR (32)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

