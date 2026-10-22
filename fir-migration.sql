IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261017133358_InitialCreate'
)
BEGIN
    CREATE TABLE [Firs] (
        [Id] uniqueidentifier NOT NULL,
        [FirNumber] varchar(30) NOT NULL,
        [PoliceStationId] nvarchar(50) NOT NULL,
        [DistrictId] nvarchar(50) NOT NULL,
        [IncidentDate] datetimeoffset NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [ComplainantName] nvarchar(200) NOT NULL,
        [Status] varchar(30) NOT NULL,
        [RegisteredAtUtc] datetimeoffset NOT NULL,
        [IncidentLocation_AddressLine1] nvarchar(250) NOT NULL,
        [IncidentLocation_AddressLine2] nvarchar(250) NULL,
        [IncidentLocation_VillageOrCity] nvarchar(100) NOT NULL,
        [IncidentLocation_District] nvarchar(100) NOT NULL,
        [IncidentLocation_State] nvarchar(100) NOT NULL,
        [IncidentLocation_PinCode] varchar(6) NULL,
        [IncidentLocation_Latitude] decimal(9,6) NULL,
        [IncidentLocation_Longitude] decimal(9,6) NULL,
        CONSTRAINT [PK_Firs] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261017133358_InitialCreate'
)
BEGIN
    CREATE TABLE [OutboxMessages] (
        [Id] uniqueidentifier NOT NULL,
        [EventType] nvarchar(200) NOT NULL,
        [EventVersion] int NOT NULL,
        [Payload] nvarchar(max) NOT NULL,
        [OccurredAtUtc] datetimeoffset NOT NULL,
        [ProcessedAtUtc] datetimeoffset NULL,
        [RetryCount] int NOT NULL,
        [LastError] nvarchar(2000) NULL,
        CONSTRAINT [PK_OutboxMessages] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261017133358_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Firs_RegisteredAtUtc] ON [Firs] ([RegisteredAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261017133358_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [UX_Firs_PoliceStation_FirNumber] ON [Firs] ([PoliceStationId], [FirNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261017133358_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_OutboxMessages_ProcessedAtUtc_OccurredAtUtc] ON [OutboxMessages] ([ProcessedAtUtc], [OccurredAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261017133358_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261017133358_InitialCreate', N'10.0.13');
END;

COMMIT;
GO

