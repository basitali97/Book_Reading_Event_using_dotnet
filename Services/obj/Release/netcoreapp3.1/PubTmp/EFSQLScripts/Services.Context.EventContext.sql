IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220509070416_MigrationV1')
BEGIN
    CREATE TABLE [Event] (
        [EventId] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [Title] nvarchar(max) NULL,
        [Date] datetime2 NOT NULL,
        [Location] nvarchar(max) NULL,
        [StartTime] datetime2 NOT NULL,
        [Type] nvarchar(max) NULL,
        [Duration] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [Details] nvarchar(max) NULL,
        [Invite] nvarchar(max) NULL,
        CONSTRAINT [PK_Event] PRIMARY KEY ([EventId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220509070416_MigrationV1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220509070416_MigrationV1', N'3.1.1');
END;

GO

