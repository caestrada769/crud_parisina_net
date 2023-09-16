using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CrudParisina.Models;

public partial class ParisinaNetContext : DbContext
{
    public ParisinaNetContext()
    {
    }

    public ParisinaNetContext(DbContextOptions<ParisinaNetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-FHFTERF;Initial Catalog=parisinaNet;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PK_categoria");

            entity.ToTable("categorias");

            entity.HasIndex(e => e.NombreCategoria, "UQ_categoria").IsUnique();

            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.DescripcionCategoria)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion_categoria");
            entity.Property(e => e.EstadoCategoria)
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado_categoria");
            entity.Property(e => e.ImagenCategoria).HasColumnName("imagen_categoria");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre_categoria");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK_cliente");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.NumeroDocumento, "UQ_numero_documento").IsUnique();

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Direccion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.EstadoClientes)
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado_clientes");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre_cliente");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("numero_documento");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telefono");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo_documento");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK_empleado");

            entity.ToTable("empleados");

            entity.HasIndex(e => e.NumeroDocumento, "UQ_numero_documento_empleado").IsUnique();

            entity.Property(e => e.IdEmpleado).HasColumnName("id_empleado");
            entity.Property(e => e.EstadoEmpleado)
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado_empleado");
            entity.Property(e => e.IdArea).HasColumnName("id_area");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.NombreEmpleado)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre_empleado");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("numero_documento");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_empleadoUsuario");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK_producto");

            entity.ToTable("productos");

            entity.HasIndex(e => e.NombreProducto, "UQ_producto").IsUnique();

            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.DescripcionProducto)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion_producto");
            entity.Property(e => e.EstadoProducto)
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado_producto");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.ImagenProducto).HasColumnName("imagen_producto");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("nombre_producto");
            entity.Property(e => e.PrecioProducto).HasColumnName("precio_producto");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK_producto_categoria");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK_usuario");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "UQ_correo").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.EstadoUsuario)
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado_usuario");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
