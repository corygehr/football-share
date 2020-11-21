CREATE TABLE [dbo].[Leagues] (
    [Id]        VARCHAR (64)   NOT NULL,
    [Name]      NVARCHAR (128) NOT NULL,
    [ShortName] NVARCHAR (32)  NULL,
    [SportId]   VARCHAR (64)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Leagues_ToSports] FOREIGN KEY ([SportId]) REFERENCES [dbo].[Sports] ([Id]) ON UPDATE CASCADE
);

