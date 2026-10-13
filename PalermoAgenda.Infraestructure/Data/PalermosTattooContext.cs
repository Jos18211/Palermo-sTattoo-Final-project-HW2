using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PalermoAgenda.Infraestructure.Models;

namespace PalermoAgenda.Infraestructure.Data;

public partial class PalermosTattooContext : DbContext
{
    public PalermosTattooContext(DbContextOptions<PalermosTattooContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Artista> Artista { get; set; }

    public virtual DbSet<Bloqueo> Bloqueo { get; set; }

    public virtual DbSet<CategoriaProducto> CategoriaProducto { get; set; }

    public virtual DbSet<Cita> Cita { get; set; }

    public virtual DbSet<EstadoCita> EstadoCita { get; set; }

    public virtual DbSet<Factura> Factura { get; set; }

    public virtual DbSet<FacturaDetalle> FacturaDetalle { get; set; }

    public virtual DbSet<Horario> Horario { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Servicio> Servicio { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artista>(entity =>
        {
            entity.HasKey(e => e.IdArtista);

            entity.HasIndex(e => e.Email, "UQ_Artista_Email").IsUnique();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Imagen)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasMany(d => d.IdServicio).WithMany(p => p.IdArtista)
                .UsingEntity<Dictionary<string, object>>(
                    "ArtistaServicio",
                    r => r.HasOne<Servicio>().WithMany()
                        .HasForeignKey("IdServicio")
                        .HasConstraintName("FK_ArtistaServicio_Servicio"),
                    l => l.HasOne<Artista>().WithMany()
                        .HasForeignKey("IdArtista")
                        .HasConstraintName("FK_ArtistaServicio_Artista"),
                    j =>
                    {
                        j.HasKey("IdArtista", "IdServicio");
                    });
        });

        modelBuilder.Entity<Bloqueo>(entity =>
        {
            entity.HasKey(e => e.IdBloqueo);

            entity.HasIndex(e => new { e.IdArtista, e.Fecha }, "IX_Bloqueo_ArtistaFecha");

            entity.Property(e => e.HoraFin).HasPrecision(0);
            entity.Property(e => e.HoraInicio).HasPrecision(0);
            entity.Property(e => e.Motivo)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.Bloqueo)
                .HasForeignKey(d => d.IdArtista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bloqueo_Artista");
        });

        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaProducto);

            entity.HasIndex(e => e.Nombre, "UQ_CategoriaProducto_Nombre").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.IdCita);

            entity.HasIndex(e => new { e.IdArtista, e.Fecha }, "IX_Cita_ArtistaFecha");

            entity.HasIndex(e => new { e.IdCliente, e.Fecha }, "IX_Cita_ClienteFecha");

            entity.HasIndex(e => e.Fecha, "IX_Cita_PendienteNotificar").HasFilter("([Notificado]=(0))");

            entity.Property(e => e.DetalleAlergias)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FechaNotificacion).HasPrecision(0);
            entity.Property(e => e.HoraFin).HasPrecision(0);
            entity.Property(e => e.HoraInicio).HasPrecision(0);
            entity.Property(e => e.IdEstadoCita).HasDefaultValue(1);
            entity.Property(e => e.ReferenciaDiseno)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ZonaCuerpo)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdArtista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cita_Artista");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cita_Cliente");

            entity.HasOne(d => d.IdEstadoCitaNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdEstadoCita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cita_EstadoCita");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cita_Servicio");
        });

        modelBuilder.Entity<EstadoCita>(entity =>
        {
            entity.HasKey(e => e.IdEstadoCita);

            entity.HasIndex(e => e.Nombre, "UQ_EstadoCita_Nombre").IsUnique();

            entity.Property(e => e.IdEstadoCita).ValueGeneratedNever();
            entity.Property(e => e.Color)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura);

            entity.HasIndex(e => new { e.IdArtista, e.FechaFactura }, "IX_Factura_ArtistaFecha");

            entity.HasIndex(e => new { e.IdCliente, e.FechaFactura }, "IX_Factura_ClienteFecha");

            entity.HasIndex(e => e.IdCita, "UX_Factura_Cita")
                .IsUnique()
                .HasFilter("([IdCita] IS NOT NULL)");

            entity.Property(e => e.Estado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Proforma");
            entity.Property(e => e.FechaFactura)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.MontoImpuesto).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.PorcentajeImpuesto)
                .HasDefaultValue(13m)
                .HasColumnType("numeric(5, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Total).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.Factura)
                .HasForeignKey(d => d.IdArtista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Artista");

            entity.HasOne(d => d.IdCitaNavigation).WithOne(p => p.Factura)
                .HasForeignKey<Factura>(d => d.IdCita)
                .HasConstraintName("FK_Factura_Cita");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Factura)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Cliente");
        });

        modelBuilder.Entity<FacturaDetalle>(entity =>
        {
            entity.HasKey(e => e.IdFacturaDetalle);

            entity.HasIndex(e => e.IdFactura, "IX_FacturaDetalle_Factura");

            entity.HasIndex(e => e.IdProducto, "IX_FacturaDetalle_Producto").HasFilter("([IdProducto] IS NOT NULL)");

            entity.HasIndex(e => e.IdServicio, "IX_FacturaDetalle_Servicio").HasFilter("([IdServicio] IS NOT NULL)");

            entity.Property(e => e.PrecioUnitario).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.IdFacturaNavigation).WithMany(p => p.FacturaDetalle)
                .HasForeignKey(d => d.IdFactura)
                .HasConstraintName("FK_FacturaDetalle_Factura");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.FacturaDetalle)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK_FacturaDetalle_Producto");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.FacturaDetalle)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK_FacturaDetalle_Servicio");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario);

            entity.HasIndex(e => new { e.IdArtista, e.DiaSemana }, "UQ_Horario_ArtistaDia").IsUnique();

            entity.Property(e => e.HoraFin).HasPrecision(0);
            entity.Property(e => e.HoraInicio).HasPrecision(0);

            entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.Horario)
                .HasForeignKey(d => d.IdArtista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Horario_Artista");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);

            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Presentacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCategoriaProductoNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdCategoriaProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_CategoriaProducto");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.HasIndex(e => e.Descripcion, "UQ_Rol_Descripcion").IsUnique();

            entity.Property(e => e.IdRol).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio);

            entity.Property(e => e.Complejidad)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Estilo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("numeric(18, 2)");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.HasIndex(e => e.Email, "UQ_Usuario_Email").IsUnique();

            entity.HasIndex(e => e.Identificacion, "UQ_Usuario_Identificacion").IsUnique();

            entity.HasIndex(e => e.IdRol, "UX_Usuario_UnicoAdministrador")
                .IsUnique()
                .HasFilter("([IdRol]=(3))");

            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaRegistro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IdRol).HasDefaultValue(1);
            entity.Property(e => e.Identificacion)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdArtista)
                .HasConstraintName("FK_Usuario_Artista");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
