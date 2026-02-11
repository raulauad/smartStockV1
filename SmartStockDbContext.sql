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
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [Dia] (
        [DiaId] int NOT NULL IDENTITY,
        [Fecha] datetime2 NOT NULL,
        CONSTRAINT [PK_Dia] PRIMARY KEY ([DiaId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [Rol] (
        [RolId] int NOT NULL IDENTITY,
        [NombreRol] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Rol] PRIMARY KEY ([RolId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [CajaDia] (
        [CajaDiaId] int NOT NULL IDENTITY,
        [DiaId] int NOT NULL,
        [TotalCierre] decimal(18,2) NOT NULL,
        [TotalGananciaDia] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_CajaDia] PRIMARY KEY ([CajaDiaId]),
        CONSTRAINT [FK_CajaDia_Dia_DiaId] FOREIGN KEY ([DiaId]) REFERENCES [Dia] ([DiaId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [Usuario] (
        [UsuarioId] int NOT NULL IDENTITY,
        [RolId] int NOT NULL,
        [UsuarioNombre] nvarchar(max) NOT NULL,
        [UsuarioDireccion] nvarchar(max) NULL,
        [UsuarioTelefono] nvarchar(max) NULL,
        [AltaUsuario] datetime2 NOT NULL,
        [UltConexion] datetime2 NULL,
        [HoraConexion] datetime2 NULL,
        [ContraseñaHash] varbinary(max) NOT NULL,
        [ContraseñaSalt] varbinary(max) NOT NULL,
        [EstadoUsuario] int NOT NULL,
        CONSTRAINT [PK_Usuario] PRIMARY KEY ([UsuarioId]),
        CONSTRAINT [FK_Usuario_Rol_RolId] FOREIGN KEY ([RolId]) REFERENCES [Rol] ([RolId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [CompraDia] (
        [CompraDiaId] int NOT NULL IDENTITY,
        [DiaId] int NOT NULL,
        [TotalCompraDia] decimal(18,2) NOT NULL,
        [CajaDiaId] int NULL,
        CONSTRAINT [PK_CompraDia] PRIMARY KEY ([CompraDiaId]),
        CONSTRAINT [FK_CompraDia_CajaDia_CajaDiaId] FOREIGN KEY ([CajaDiaId]) REFERENCES [CajaDia] ([CajaDiaId]),
        CONSTRAINT [FK_CompraDia_Dia_DiaId] FOREIGN KEY ([DiaId]) REFERENCES [Dia] ([DiaId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [VentaDia] (
        [VentaDiaId] int NOT NULL IDENTITY,
        [DiaId] int NOT NULL,
        [TotalVentaDia] decimal(18,2) NOT NULL,
        [TotalGananciaVenta] decimal(18,2) NOT NULL,
        [CajaDiaId] int NULL,
        CONSTRAINT [PK_VentaDia] PRIMARY KEY ([VentaDiaId]),
        CONSTRAINT [FK_VentaDia_CajaDia_CajaDiaId] FOREIGN KEY ([CajaDiaId]) REFERENCES [CajaDia] ([CajaDiaId]),
        CONSTRAINT [FK_VentaDia_Dia_DiaId] FOREIGN KEY ([DiaId]) REFERENCES [Dia] ([DiaId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [Categoria] (
        [CategoriaId] int NOT NULL IDENTITY,
        [UsuarioId] int NOT NULL,
        [Nombre] nvarchar(max) NOT NULL,
        [FechaCreacionCategoria] datetime2 NOT NULL,
        CONSTRAINT [PK_Categoria] PRIMARY KEY ([CategoriaId]),
        CONSTRAINT [FK_Categoria_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuario] ([UsuarioId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [Proveedor] (
        [ProveedorId] int NOT NULL IDENTITY,
        [UsuarioId] int NOT NULL,
        [Nombre] nvarchar(max) NOT NULL,
        [RazonSocial] nvarchar(max) NOT NULL,
        [Cuit] nvarchar(max) NOT NULL,
        [DireccionProveedor] nvarchar(max) NOT NULL,
        [TelefonoProveedor] nvarchar(max) NOT NULL,
        [FechaCreacionProveedor] datetime2 NOT NULL,
        [EstadoProveedor] bit NOT NULL,
        CONSTRAINT [PK_Proveedor] PRIMARY KEY ([ProveedorId]),
        CONSTRAINT [FK_Proveedor_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuario] ([UsuarioId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [DetalleVenta] (
        [DetalleVentaId] int NOT NULL IDENTITY,
        [VentaDiaId] int NOT NULL,
        [UsuarioId] int NOT NULL,
        [SubtotalDetalleVenta] decimal(18,2) NOT NULL,
        [FechDetalleVenta] datetime2 NOT NULL,
        CONSTRAINT [PK_DetalleVenta] PRIMARY KEY ([DetalleVentaId]),
        CONSTRAINT [FK_DetalleVenta_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuario] ([UsuarioId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_DetalleVenta_VentaDia_VentaDiaId] FOREIGN KEY ([VentaDiaId]) REFERENCES [VentaDia] ([VentaDiaId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [DetalleCompra] (
        [DetalleCompraId] int NOT NULL IDENTITY,
        [CompraDiaId] int NOT NULL,
        [ProveedorId] int NOT NULL,
        [UsuarioId] int NOT NULL,
        [SubtotalDetalleCompra] decimal(18,2) NOT NULL,
        [FechaDetalleCompra] datetime2 NOT NULL,
        CONSTRAINT [PK_DetalleCompra] PRIMARY KEY ([DetalleCompraId]),
        CONSTRAINT [FK_DetalleCompra_CompraDia_CompraDiaId] FOREIGN KEY ([CompraDiaId]) REFERENCES [CompraDia] ([CompraDiaId]) ON DELETE CASCADE,
        CONSTRAINT [FK_DetalleCompra_Proveedor_ProveedorId] FOREIGN KEY ([ProveedorId]) REFERENCES [Proveedor] ([ProveedorId]) ON DELETE CASCADE,
        CONSTRAINT [FK_DetalleCompra_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuario] ([UsuarioId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [DetalleCompraItem] (
        [CompraItemId] int NOT NULL IDENTITY,
        [DetalleCompraId] int NOT NULL,
        [ProductoId] int NOT NULL,
        [CantidadItem] decimal(18,2) NOT NULL,
        [PrecioCostoItem] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_DetalleCompraItem] PRIMARY KEY ([CompraItemId]),
        CONSTRAINT [FK_DetalleCompraItem_DetalleCompra_DetalleCompraId] FOREIGN KEY ([DetalleCompraId]) REFERENCES [DetalleCompra] ([DetalleCompraId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [DetalleVentaItem] (
        [VentaItemId] int NOT NULL IDENTITY,
        [DetalleVentaId] int NOT NULL,
        [ProductoId] int NOT NULL,
        [CantidadItem] decimal(18,2) NOT NULL,
        [PrecioCostoItem] decimal(18,2) NOT NULL,
        [PrecioVentaItem] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_DetalleVentaItem] PRIMARY KEY ([VentaItemId]),
        CONSTRAINT [FK_DetalleVentaItem_DetalleVenta_DetalleVentaId] FOREIGN KEY ([DetalleVentaId]) REFERENCES [DetalleVenta] ([DetalleVentaId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [MovimientoStock] (
        [MovimientoId] int NOT NULL IDENTITY,
        [ProductoId] int NOT NULL,
        [TipoMovimiento] nvarchar(max) NOT NULL,
        [CompraItemId] int NULL,
        [VentaItemId] int NULL,
        [Cantidad] decimal(18,2) NOT NULL,
        [MotivoAjuste] nvarchar(max) NOT NULL,
        [FechaMovimiento] datetime2 NOT NULL,
        CONSTRAINT [PK_MovimientoStock] PRIMARY KEY ([MovimientoId]),
        CONSTRAINT [FK_MovimientoStock_DetalleCompraItem_CompraItemId] FOREIGN KEY ([CompraItemId]) REFERENCES [DetalleCompraItem] ([CompraItemId]),
        CONSTRAINT [FK_MovimientoStock_DetalleVentaItem_VentaItemId] FOREIGN KEY ([VentaItemId]) REFERENCES [DetalleVentaItem] ([VentaItemId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [Producto] (
        [ProductoId] int NOT NULL IDENTITY,
        [CategoriaId] int NOT NULL,
        [UsuarioId] int NOT NULL,
        [Nombre] nvarchar(max) NOT NULL,
        [CodigoBarra] nvarchar(max) NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        [PrecioCosto] decimal(18,2) NOT NULL,
        [PrecioVenta] decimal(18,2) NOT NULL,
        [EstadoProducto] bit NOT NULL,
        [StockActualProductoId] int NULL,
        CONSTRAINT [PK_Producto] PRIMARY KEY ([ProductoId]),
        CONSTRAINT [FK_Producto_Categoria_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categoria] ([CategoriaId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Producto_Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuario] ([UsuarioId]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE TABLE [StockActual] (
        [ProductoId] int NOT NULL,
        [CantidadStock] int NOT NULL,
        [UltActualizacion] datetime2 NOT NULL,
        CONSTRAINT [PK_StockActual] PRIMARY KEY ([ProductoId]),
        CONSTRAINT [FK_StockActual_Producto_ProductoId] FOREIGN KEY ([ProductoId]) REFERENCES [Producto] ([ProductoId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RolId', N'NombreRol') AND [object_id] = OBJECT_ID(N'[Rol]'))
        SET IDENTITY_INSERT [Rol] ON;
    EXEC(N'INSERT INTO [Rol] ([RolId], [NombreRol])
    VALUES (1, N''Admin''),
    (2, N''Usuario'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RolId', N'NombreRol') AND [object_id] = OBJECT_ID(N'[Rol]'))
        SET IDENTITY_INSERT [Rol] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_CajaDia_DiaId] ON [CajaDia] ([DiaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Categoria_UsuarioId] ON [Categoria] ([UsuarioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CompraDia_CajaDiaId] ON [CompraDia] ([CajaDiaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_CompraDia_DiaId] ON [CompraDia] ([DiaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DetalleCompra_CompraDiaId] ON [DetalleCompra] ([CompraDiaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DetalleCompra_ProveedorId] ON [DetalleCompra] ([ProveedorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DetalleCompra_UsuarioId] ON [DetalleCompra] ([UsuarioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DetalleCompraItem_DetalleCompraId] ON [DetalleCompraItem] ([DetalleCompraId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DetalleCompraItem_ProductoId] ON [DetalleCompraItem] ([ProductoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DetalleVenta_UsuarioId] ON [DetalleVenta] ([UsuarioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DetalleVenta_VentaDiaId] ON [DetalleVenta] ([VentaDiaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DetalleVentaItem_DetalleVentaId] ON [DetalleVentaItem] ([DetalleVentaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DetalleVentaItem_ProductoId] ON [DetalleVentaItem] ([ProductoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_MovimientoStock_CompraItemId] ON [MovimientoStock] ([CompraItemId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_MovimientoStock_ProductoId] ON [MovimientoStock] ([ProductoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_MovimientoStock_VentaItemId] ON [MovimientoStock] ([VentaItemId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Producto_CategoriaId] ON [Producto] ([CategoriaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Producto_StockActualProductoId] ON [Producto] ([StockActualProductoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Producto_UsuarioId] ON [Producto] ([UsuarioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Proveedor_UsuarioId] ON [Proveedor] ([UsuarioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Usuario_RolId] ON [Usuario] ([RolId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_VentaDia_CajaDiaId] ON [VentaDia] ([CajaDiaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_VentaDia_DiaId] ON [VentaDia] ([DiaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    ALTER TABLE [DetalleCompraItem] ADD CONSTRAINT [FK_DetalleCompraItem_Producto_ProductoId] FOREIGN KEY ([ProductoId]) REFERENCES [Producto] ([ProductoId]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    ALTER TABLE [DetalleVentaItem] ADD CONSTRAINT [FK_DetalleVentaItem_Producto_ProductoId] FOREIGN KEY ([ProductoId]) REFERENCES [Producto] ([ProductoId]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    ALTER TABLE [MovimientoStock] ADD CONSTRAINT [FK_MovimientoStock_Producto_ProductoId] FOREIGN KEY ([ProductoId]) REFERENCES [Producto] ([ProductoId]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    ALTER TABLE [Producto] ADD CONSTRAINT [FK_Producto_StockActual_StockActualProductoId] FOREIGN KEY ([StockActualProductoId]) REFERENCES [StockActual] ([ProductoId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251128202330_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251128202330_InitialCreate', N'9.0.10');
END;

COMMIT;
GO