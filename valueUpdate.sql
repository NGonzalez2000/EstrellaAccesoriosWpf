Build started...
Build succeeded.
BEGIN TRANSACTION;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Providers]'))
    SET IDENTITY_INSERT [Providers] ON;
INSERT INTO [Providers] ([Id], [Name])
VALUES ('93f4bd05-cd65-4cc8-89d6-84a06dcd0166', N'SIN PROVEEDOR');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Providers]'))
    SET IDENTITY_INSERT [Providers] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description') AND [object_id] = OBJECT_ID(N'[Categories]'))
    SET IDENTITY_INSERT [Categories] ON;
INSERT INTO [Categories] ([Id], [Description])
VALUES ('4bf5157b-7e2d-4bed-8199-31576498bfc4', N'SIN CATEGORIA');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description') AND [object_id] = OBJECT_ID(N'[Categories]'))
    SET IDENTITY_INSERT [Categories] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240622165402_CustomValues', N'8.0.1');
GO

COMMIT;
GO


