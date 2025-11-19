using Microsoft.EntityFrameworkCore;
using SmartStockV1.Models;

namespace SmartStockV1.Data
{
    public class SmartStockDbContext : DbContext
    {
        public SmartStockDbContext(DbContextOptions<SmartStockDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Categoria> Categoria { get; set; } 
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<DetalleCompra> DetalleCompra { get; set; }
        public DbSet<DetalleCompraItem> DetalleCompraItem { get; set; }
        public DbSet<DetalleVenta> DetalleVenta { get; set; }
        public DbSet<DetalleVentaItem> DetalleVentaItem { get; set; }
        public DbSet<MovimientoStock> MovimientoStock { get; set; }
        public DbSet<StockActual> StockActual { get; set; }
        public DbSet<CompraDia> CompraDia { get; set; }
        public DbSet<VentaDia> VentaDia { get; set; }
        public DbSet<CajaDia> CajaDia { get; set; }
        public DbSet<Dia> Dia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rol>().HasData(
                new Rol { RolId = (int)TipoRol.Admin, NombreRol = "Admin" },
                new Rol { RolId = (int)TipoRol.Usuario, NombreRol = "Usuario" }
            );

            modelBuilder.Entity<Usuario>().HasKey(u => u.UsuarioId);
            modelBuilder.Entity<Rol>().HasKey(r => r.RolId);
            modelBuilder.Entity<Categoria>().HasKey(c => c.CategoriaId);
            modelBuilder.Entity<Proveedor>().HasKey(p => p.ProveedorId);
            modelBuilder.Entity<Producto>().HasKey(p => p.ProductoId);
            modelBuilder.Entity<DetalleCompra>().HasKey(dc => dc.DetalleCompraId);
            modelBuilder.Entity<DetalleCompraItem>().HasKey(dci => dci.CompraItemId);
            modelBuilder.Entity<DetalleVenta>().HasKey(dv => dv.DetalleVentaId);
            modelBuilder.Entity<DetalleVentaItem>().HasKey(dvi => dvi.VentaItemId);
            modelBuilder.Entity<MovimientoStock>().HasKey(ms => ms.MovimientoId);
            modelBuilder.Entity<StockActual>().HasKey(sa => sa.ProductoId);
            modelBuilder.Entity<CompraDia>().HasKey(cd => cd.CompraDiaId);
            modelBuilder.Entity<VentaDia>().HasKey(vd => vd.VentaDiaId);
            modelBuilder.Entity<CajaDia>().HasKey(cj => cj.CajaDiaId);
            modelBuilder.Entity<Dia>().HasKey(d => d.DiaId);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.UsuariosCreados)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Categoria>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.CategoriasCreadas)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Proveedor>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.ProveedoresCreados)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Usuario)
                .WithMany(u => u.DetallesCompra)
                .HasForeignKey(dc => dc.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Usuario)
                .WithMany(u => u.DetallesVenta)
                .HasForeignKey(dv => dv.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.ProductosCreados)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleCompraItem>()
                .HasOne(dci => dci.Producto)
                .WithMany(p => p.DetallesCompraItem)
                .HasForeignKey(dci => dci.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleVentaItem>()
                .HasOne(dvi => dvi.Producto)
                .WithMany(p => p.DetallesVentaItem)
                .HasForeignKey(dvi => dvi.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MovimientoStock>()
                .HasOne(ms => ms.Producto)
                .WithMany(p => p.MovimientosStock)
                .HasForeignKey(ms => ms.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockActual>()
                .HasOne(sa => sa.Producto)
                .WithOne()
                .HasForeignKey<StockActual>(sa => sa.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetalleCompraItem>()
                .HasOne(dci => dci.DetalleCompra)
                .WithMany(dc => dc.ItemsDetalleCompra)
                .HasForeignKey(dci => dci.DetalleCompraId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetalleVentaItem>()
                .HasOne(dvi => dvi.DetalleVenta)
                .WithMany(dv => dv.ItemsDetalleVenta)
                .HasForeignKey(dvi => dvi.DetalleVentaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompraDia>()
                .HasOne(cd => cd.Dia)
                .WithOne(d => d.CompraDia)
                .HasForeignKey<CompraDia>(cd => cd.DiaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VentaDia>()
                .HasOne(vd => vd.Dia)
                .WithOne(d => d.VentaDia)
                .HasForeignKey<VentaDia>(vd => vd.DiaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CajaDia>()
                .HasOne(cj => cj.Dia)
                .WithOne(d => d.CajaDia)
                .HasForeignKey<CajaDia>(cj => cj.DiaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Producto>().Property(p => p.PrecioCosto).HasPrecision(18, 2);
            modelBuilder.Entity<Producto>().Property(p => p.PrecioVenta).HasPrecision(18, 2);

            modelBuilder.Entity<DetalleCompraItem>().Property(d => d.CantidadItem).HasPrecision(18, 2);
            modelBuilder.Entity<DetalleCompraItem>().Property(d => d.PrecioCostoItem).HasPrecision(18, 2);

            modelBuilder.Entity<DetalleVentaItem>().Property(d => d.PrecioVentaItem).HasPrecision(18, 2);
            modelBuilder.Entity<DetalleVentaItem>().Property(d => d.CantidadItem).HasPrecision(18, 2);

            modelBuilder.Entity<CajaDia>().Property(c => c.TotalCierre).HasPrecision(18, 2);
            modelBuilder.Entity<CajaDia>().Property(c => c.TotalGananciaDia).HasPrecision(18, 2);

            modelBuilder.Entity<CompraDia>().Property(cd => cd.TotalCompraDia).HasPrecision(18, 2);
            modelBuilder.Entity<VentaDia>().Property(vd => vd.TotalVentaDia).HasPrecision(18, 2);
            modelBuilder.Entity<VentaDia>().Property(vd => vd.TotalGananciaVenta).HasPrecision(18, 2);

            modelBuilder.Entity<DetalleCompra>().Property(dc => dc.SubtotalDetalleCompra).HasPrecision(18, 2);

            modelBuilder.Entity<DetalleVenta>().Property(dv => dv.SubtotalDetalleVenta).HasPrecision(18, 2);

            modelBuilder.Entity<DetalleVentaItem>().Property(dvi => dvi.PrecioCostoItem).HasPrecision(18, 2);

            modelBuilder.Entity<MovimientoStock>().Property(ms => ms.Cantidad).HasPrecision(18, 2);
        }
    }
}

