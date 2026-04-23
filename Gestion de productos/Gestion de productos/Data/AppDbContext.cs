using Microsoft.EntityFrameworkCore;
using Gestion_de_productos.Models;

namespace Gestion_de_productos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets para todas las entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ImagenProducto> ImagenesProducto { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoItem> CarritoItems { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalle> PedidoDetalles { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Envio> Envios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== Configuración de Rol ==========
            modelBuilder.Entity<Rol>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Rol>()
                .Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            // ========== Configuración de Usuario ==========
            modelBuilder.Entity<Usuario>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Contraseña)
                .IsRequired();

            // Relación Usuario - Rol (Many to One)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Usuario - Carrito (One to One)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Carrito)
                .WithOne(c => c.Usuario)
                .HasForeignKey<Carrito>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Usuario - Pedido (One to Many)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Pedidos)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== Configuración de Carrito ==========
            modelBuilder.Entity<Carrito>()
                .HasKey(c => c.Id);

            // Relación Carrito - CarritoItem (One to Many)
            modelBuilder.Entity<Carrito>()
                .HasMany(c => c.Items)
                .WithOne(ci => ci.Carrito)
                .HasForeignKey(ci => ci.CarritoId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== Configuración de CarritoItem ==========
            modelBuilder.Entity<CarritoItem>()
                .HasKey(ci => ci.Id);

            modelBuilder.Entity<CarritoItem>()
                .Property(ci => ci.Cantidad)
                .IsRequired();

            // Relación CarritoItem - Producto (Many to One)
            modelBuilder.Entity<CarritoItem>()
                .HasOne(ci => ci.Producto)
                .WithMany()
                .HasForeignKey(ci => ci.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== Configuración de Categoria ==========
            modelBuilder.Entity<Categoria>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Categoria>()
                .Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            // Relación Categoria - Producto (One to Many)
            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // ========== Configuración de Producto ==========
            modelBuilder.Entity<Producto>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Descripcion)
                .HasMaxLength(1000);

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(10, 2)
                .IsRequired();

            modelBuilder.Entity<Producto>()
                .Property(p => p.Stock)
                .IsRequired();

            // Relación Producto - ImagenProducto (One to Many)
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Imagenes)
                .WithOne(i => i.Producto)
                .HasForeignKey(i => i.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== Configuración de ImagenProducto ==========
            modelBuilder.Entity<ImagenProducto>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<ImagenProducto>()
                .Property(i => i.Url)
                .IsRequired()
                .HasMaxLength(500);

            // ========== Configuración de Pedido ==========
            modelBuilder.Entity<Pedido>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Fecha)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Estado)
                .IsRequired()
                .HasMaxLength(50);

            // Relación Pedido - PedidoDetalle (One to Many)
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Detalles)
                .WithOne(pd => pd.Pedido)
                .HasForeignKey(pd => pd.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Pedido - Pago (One to One)
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Pago)
                .WithOne(pa => pa.Pedido)
                .HasForeignKey<Pago>(pa => pa.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Pedido - Envío (One to One)
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Envio)
                .WithOne(e => e.Pedido)
                .HasForeignKey<Envio>(e => e.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== Configuración de PedidoDetalle ==========
            modelBuilder.Entity<PedidoDetalle>()
                .HasKey(pd => pd.Id);

            modelBuilder.Entity<PedidoDetalle>()
                .Property(pd => pd.Cantidad)
                .IsRequired();

            modelBuilder.Entity<PedidoDetalle>()
                .Property(pd => pd.PrecioUnitario)
                .HasPrecision(10, 2);

            // ========== Configuración de Pago ==========
            modelBuilder.Entity<Pago>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Pago>()
                .Property(p => p.Monto)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pago>()
                .Property(p => p.FormaPago)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Pago>()
                .Property(p => p.Estado)
                .IsRequired()
                .HasMaxLength(50);

            // ========== Configuración de Envío ==========
            modelBuilder.Entity<Envio>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Envio>()
                .Property(e => e.Direccion)
                .IsRequired()
                .HasMaxLength(300);

            modelBuilder.Entity<Envio>()
                .Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
