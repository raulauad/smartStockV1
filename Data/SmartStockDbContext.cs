using Microsoft.EntityFrameworkCore;
using SmartStockV1.Models;


namespace SmartStockV1.Data
{
    public class SmartStockDbContext : DbContext
    {
        public SmartStockDbContext(DbContextOptions<SmartStockDbContext> options) :base(options) { }

        //DbSets
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
            //Configuraciones adicionales

            //Seeds de roles base
            modelBuilder.Entity<Rol>().HasData(
                new Rol { RolId = (int)TipoRol.Admin, Nombre = "Admin" },
                new Rol { RolId = (int)TipoRol.Usuario, Nombre = "Usuario" }
            );

            //Claves Primarias
            modelBuilder.Entity<Usuario>() //Usuario
                .HasKey(u => u.UsuarioId);

            modelBuilder.Entity<Rol>() //Rol
                .HasKey(r => r.RolId);

            modelBuilder.Entity<Categoria>() //Categoria
                .HasKey(c => c.CategoriaId);

            modelBuilder.Entity<Proveedor>() //Proveedor
                .HasKey(p => p.ProveedorId);

            modelBuilder.Entity<Producto>() //Producto
                .HasKey(p => p.ProductoId);

            modelBuilder.Entity<DetalleCompra>() //DetalleCompra
                .HasKey(dc => dc.DetalleCompraId);

            modelBuilder.Entity<DetalleVenta>() //DetalleVenta
                .HasKey(dv => dv.DetalleVentaId);

            modelBuilder.Entity<DetalleCompraItem>() //DetalleCompraItem
                .HasKey(dci => dci.CompraItemId);

            modelBuilder.Entity<DetalleVentaItem>() //DetalleVentaItem
                .HasKey(dvi => dvi.VentaItemId);

            modelBuilder.Entity<MovimientoStock>() //MovimientoStock
                .HasKey(ms => ms.MovimientoId);

            modelBuilder.Entity<StockActual>() //StockActual
                .HasKey(sa => sa.ProductoId);

            modelBuilder.Entity<Dia>() //Dia
                .HasKey(d => d.DiaId);

            modelBuilder.Entity<CompraDia>() //CompraDia
                .HasKey(cd => cd.CompraDiaId);

            modelBuilder.Entity<VentaDia>() //VentaDia
                .HasKey(vd => vd.VentaDiaId);

            modelBuilder.Entity<CajaDia>() //CajaDia
                .HasKey(cd => cd.CajaDiaId);

            //----- Relaciones -----
            //Rol - Usuario (1 a N)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.UsuariosCreados)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            //Admin - Categoria (1 a N)
            modelBuilder.Entity<Categoria>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.CategoriasCreadas)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            //Admin - Proveedor (1 a N)
            modelBuilder.Entity<Proveedor>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.ProveedoresCreados)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            //Categoria - Producto (1 a N)
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            //Usuario - Producto (1 a N)
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.ProductosCreados)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            //Usuario - DetalleCompra (1 a N)
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Usuario)
                .WithMany(u => u.DetallesCompra)
                .HasForeignKey(dc => dc.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            //Proveedor - DetalleCompra (1 a N)
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Proveedor)
                .WithMany(p => p.DetallesCompra)
                .HasForeignKey(dc => dc.ProveedorId)
                .OnDelete(DeleteBehavior.Restrict);

            //DetalleCompra - DetalleCompraItem (1 a N)
            modelBuilder.Entity<DetalleCompraItem>()
                .HasOne(dci => dci.DetalleCompra)
                .WithMany(dc => dc.ItemsDetalleCompra)
                .HasForeignKey(dci => dci.DetalleCompraId)
                .OnDelete(DeleteBehavior.Restrict);

            //Producto - DetalleCompraItem (1 a N)
            modelBuilder.Entity<DetalleCompraItem>()
                .HasOne(dci => dci.Producto)
                .WithMany(p => p.DetallesCompraItem)
                .HasForeignKey(dci => dci.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            //Usuario - DetalleVenta (1 a N)
            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Usuario)
                .WithMany(u => u.DetallesVenta)
                .HasForeignKey(dv => dv.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            //DetalleVenta - DetalleVentaItem (1 a N)
            modelBuilder.Entity<DetalleVentaItem>()
                .HasOne(dvi => dvi.DetalleVenta)
                .WithMany(dv => dv.ItemsDetalleVenta)
                .HasForeignKey(dvi => dvi.DetalleVentaId)
                .OnDelete(DeleteBehavior.Restrict);

            //Producto - DetalleVentaItem (1 a N)
            modelBuilder.Entity<DetalleVentaItem>()
                .HasOne(dvi => dvi.Producto)
                .WithMany()
                .HasForeignKey(dvi => dvi.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            //Producto - MovimientoStock (1 a N)
            modelBuilder.Entity<MovimientoStock>()
                .HasOne(ms => ms.Producto)
                .WithMany()
                .HasForeignKey(ms => ms.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            //Producto - StockActual (1 a 1)
            modelBuilder.Entity<StockActual>()
                .HasOne(sa => sa.Producto)
                .WithOne()
                .HasForeignKey<StockActual>(sa => sa.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

            //----- Relaciones Diarias -----

            // Dia - CompraDia (1 a 1)
            modelBuilder.Entity<CompraDia>()
                .HasMany(cd => cd.DetallesCompra)
                .WithOne()
                .HasForeignKey(dc => dc.CompraDiaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Dia - VentaDia (1 a 1)
            modelBuilder.Entity<VentaDia>()
                .HasMany(vd => vd.DetallesVentas)
                .WithOne()
                .HasForeignKey(dv => dv.VentaDiaId)
                .OnDelete(DeleteBehavior.Cascade);

            // CompraDia - CajaDia (1 a 1)
            modelBuilder.Entity<CajaDia>()
                .HasOne(cd => cd.CompraDia)
                .WithOne()
                .HasForeignKey<CajaDia>(cd => cd.CompraDiaId)
                .OnDelete(DeleteBehavior.Restrict);

            // VentaDia - CajaDia (1 a 1)
            modelBuilder.Entity<CajaDia>()
                .HasOne(cd => cd.VentaDia)
                .WithOne()
                .HasForeignKey<CajaDia>(cd => cd.VentaDiaId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }

}
