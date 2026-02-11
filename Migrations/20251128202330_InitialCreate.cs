using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SmartStockV1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dia",
                columns: table => new
                {
                    DiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dia", x => x.DiaId);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    RolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.RolId);
                });

            migrationBuilder.CreateTable(
                name: "CajaDia",
                columns: table => new
                {
                    CajaDiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiaId = table.Column<int>(type: "int", nullable: false),
                    TotalCierre = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalGananciaDia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CajaDia", x => x.CajaDiaId);
                    table.ForeignKey(
                        name: "FK_CajaDia_Dia_DiaId",
                        column: x => x.DiaId,
                        principalTable: "Dia",
                        principalColumn: "DiaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    UsuarioNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioDireccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioTelefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AltaUsuario = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltConexion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HoraConexion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContraseñaHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ContraseñaSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EstadoUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_Usuario_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompraDia",
                columns: table => new
                {
                    CompraDiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiaId = table.Column<int>(type: "int", nullable: false),
                    TotalCompraDia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CajaDiaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraDia", x => x.CompraDiaId);
                    table.ForeignKey(
                        name: "FK_CompraDia_CajaDia_CajaDiaId",
                        column: x => x.CajaDiaId,
                        principalTable: "CajaDia",
                        principalColumn: "CajaDiaId");
                    table.ForeignKey(
                        name: "FK_CompraDia_Dia_DiaId",
                        column: x => x.DiaId,
                        principalTable: "Dia",
                        principalColumn: "DiaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VentaDia",
                columns: table => new
                {
                    VentaDiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiaId = table.Column<int>(type: "int", nullable: false),
                    TotalVentaDia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalGananciaVenta = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CajaDiaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaDia", x => x.VentaDiaId);
                    table.ForeignKey(
                        name: "FK_VentaDia_CajaDia_CajaDiaId",
                        column: x => x.CajaDiaId,
                        principalTable: "CajaDia",
                        principalColumn: "CajaDiaId");
                    table.ForeignKey(
                        name: "FK_VentaDia_Dia_DiaId",
                        column: x => x.DiaId,
                        principalTable: "Dia",
                        principalColumn: "DiaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacionCategoria = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.CategoriaId);
                    table.ForeignKey(
                        name: "FK_Categoria_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    ProveedorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cuit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionProveedor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefonoProveedor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacionProveedor = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoProveedor = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.ProveedorId);
                    table.ForeignKey(
                        name: "FK_Proveedor_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetalleVenta",
                columns: table => new
                {
                    DetalleVentaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VentaDiaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    SubtotalDetalleVenta = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechDetalleVenta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleVenta", x => x.DetalleVentaId);
                    table.ForeignKey(
                        name: "FK_DetalleVenta_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleVenta_VentaDia_VentaDiaId",
                        column: x => x.VentaDiaId,
                        principalTable: "VentaDia",
                        principalColumn: "VentaDiaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleCompra",
                columns: table => new
                {
                    DetalleCompraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompraDiaId = table.Column<int>(type: "int", nullable: false),
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    SubtotalDetalleCompra = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaDetalleCompra = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleCompra", x => x.DetalleCompraId);
                    table.ForeignKey(
                        name: "FK_DetalleCompra_CompraDia_CompraDiaId",
                        column: x => x.CompraDiaId,
                        principalTable: "CompraDia",
                        principalColumn: "CompraDiaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleCompra_Proveedor_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedor",
                        principalColumn: "ProveedorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleCompra_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetalleCompraItem",
                columns: table => new
                {
                    CompraItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetalleCompraId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    CantidadItem = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PrecioCostoItem = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleCompraItem", x => x.CompraItemId);
                    table.ForeignKey(
                        name: "FK_DetalleCompraItem_DetalleCompra_DetalleCompraId",
                        column: x => x.DetalleCompraId,
                        principalTable: "DetalleCompra",
                        principalColumn: "DetalleCompraId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleVentaItem",
                columns: table => new
                {
                    VentaItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetalleVentaId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    CantidadItem = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PrecioCostoItem = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PrecioVentaItem = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleVentaItem", x => x.VentaItemId);
                    table.ForeignKey(
                        name: "FK_DetalleVentaItem_DetalleVenta_DetalleVentaId",
                        column: x => x.DetalleVentaId,
                        principalTable: "DetalleVenta",
                        principalColumn: "DetalleVentaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimientoStock",
                columns: table => new
                {
                    MovimientoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompraItemId = table.Column<int>(type: "int", nullable: true),
                    VentaItemId = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MotivoAjuste = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaMovimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoStock", x => x.MovimientoId);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_DetalleCompraItem_CompraItemId",
                        column: x => x.CompraItemId,
                        principalTable: "DetalleCompraItem",
                        principalColumn: "CompraItemId");
                    table.ForeignKey(
                        name: "FK_MovimientoStock_DetalleVentaItem_VentaItemId",
                        column: x => x.VentaItemId,
                        principalTable: "DetalleVentaItem",
                        principalColumn: "VentaItemId");
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    ProductoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoBarra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecioCosto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PrecioVenta = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EstadoProducto = table.Column<bool>(type: "bit", nullable: false),
                    StockActualProductoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.ProductoId);
                    table.ForeignKey(
                        name: "FK_Producto_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Producto_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockActual",
                columns: table => new
                {
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    CantidadStock = table.Column<int>(type: "int", nullable: false),
                    UltActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockActual", x => x.ProductoId);
                    table.ForeignKey(
                        name: "FK_StockActual_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "ProductoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "RolId", "NombreRol" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Usuario" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CajaDia_DiaId",
                table: "CajaDia",
                column: "DiaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_UsuarioId",
                table: "Categoria",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_CompraDia_CajaDiaId",
                table: "CompraDia",
                column: "CajaDiaId");

            migrationBuilder.CreateIndex(
                name: "IX_CompraDia_DiaId",
                table: "CompraDia",
                column: "DiaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompra_CompraDiaId",
                table: "DetalleCompra",
                column: "CompraDiaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompra_ProveedorId",
                table: "DetalleCompra",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompra_UsuarioId",
                table: "DetalleCompra",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompraItem_DetalleCompraId",
                table: "DetalleCompraItem",
                column: "DetalleCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompraItem_ProductoId",
                table: "DetalleCompraItem",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleVenta_UsuarioId",
                table: "DetalleVenta",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleVenta_VentaDiaId",
                table: "DetalleVenta",
                column: "VentaDiaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleVentaItem_DetalleVentaId",
                table: "DetalleVentaItem",
                column: "DetalleVentaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleVentaItem_ProductoId",
                table: "DetalleVentaItem",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_CompraItemId",
                table: "MovimientoStock",
                column: "CompraItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_ProductoId",
                table: "MovimientoStock",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_VentaItemId",
                table: "MovimientoStock",
                column: "VentaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CategoriaId",
                table: "Producto",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_StockActualProductoId",
                table: "Producto",
                column: "StockActualProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_UsuarioId",
                table: "Producto",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_UsuarioId",
                table: "Proveedor",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_RolId",
                table: "Usuario",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDia_CajaDiaId",
                table: "VentaDia",
                column: "CajaDiaId");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDia_DiaId",
                table: "VentaDia",
                column: "DiaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleCompraItem_Producto_ProductoId",
                table: "DetalleCompraItem",
                column: "ProductoId",
                principalTable: "Producto",
                principalColumn: "ProductoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVentaItem_Producto_ProductoId",
                table: "DetalleVentaItem",
                column: "ProductoId",
                principalTable: "Producto",
                principalColumn: "ProductoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientoStock_Producto_ProductoId",
                table: "MovimientoStock",
                column: "ProductoId",
                principalTable: "Producto",
                principalColumn: "ProductoId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_StockActual_StockActualProductoId",
                table: "Producto",
                column: "StockActualProductoId",
                principalTable: "StockActual",
                principalColumn: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categoria_Usuario_UsuarioId",
                table: "Categoria");

            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Usuario_UsuarioId",
                table: "Producto");

            migrationBuilder.DropForeignKey(
                name: "FK_StockActual_Producto_ProductoId",
                table: "StockActual");

            migrationBuilder.DropTable(
                name: "MovimientoStock");

            migrationBuilder.DropTable(
                name: "DetalleCompraItem");

            migrationBuilder.DropTable(
                name: "DetalleVentaItem");

            migrationBuilder.DropTable(
                name: "DetalleCompra");

            migrationBuilder.DropTable(
                name: "DetalleVenta");

            migrationBuilder.DropTable(
                name: "CompraDia");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropTable(
                name: "VentaDia");

            migrationBuilder.DropTable(
                name: "CajaDia");

            migrationBuilder.DropTable(
                name: "Dia");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "StockActual");
        }
    }
}
