using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dealer_API.Models
{
    public partial class DealerContext : DbContext
    {
        public DealerContext()
        {
        }

        public DealerContext(DbContextOptions<DealerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<TipoTransaccion> TipoTransaccions { get; set; } = null!;
        public virtual DbSet<Transaccion> Transaccions { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Vehiculo> Vehiculos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-5L2SIUP; Database=Dealer; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("Rol");

                entity.Property(e => e.RolId).HasColumnName("RolID");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoTransaccion>(entity =>
            {
                entity.ToTable("TipoTransaccion");

                entity.Property(e => e.TipoTransaccionId).HasColumnName("TipoTransaccionID");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transaccion>(entity =>
            {
                entity.ToTable("Transaccion");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.FechaTransaccion).HasColumnType("datetime");

                entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TipoTransaccionId).HasColumnName("TipoTransaccionID");

                entity.Property(e => e.VehiculoId).HasColumnName("VehiculoID");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Transaccions)
                    .HasForeignKey(d => d.ClienteId)
                    .HasConstraintName("FK__Transacci__Clien__31EC6D26");

                entity.HasOne(d => d.TipoTransaccion)
                    .WithMany(p => p.Transaccions)
                    .HasForeignKey(d => d.TipoTransaccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transacci__TipoT__300424B4");

                entity.HasOne(d => d.Vehiculo)
                    .WithMany(p => p.Transaccions)
                    .HasForeignKey(d => d.VehiculoId)
                    .HasConstraintName("FK__Transacci__Vehic__30F848ED");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.HasIndex(e => e.Correo, "UQ__Usuario__60695A19E2398F0B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Celular)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RolId).HasColumnName("RolID");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.RolId)
                    .HasConstraintName("FK__Usuario__RolID__276EDEB3");
            });

            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.ToTable("Vehiculo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Color)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Condicion)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Imagen)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Marca)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Modelo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PropietarioId).HasColumnName("PropietarioID");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Propietario)
                    .WithMany(p => p.Vehiculos)
                    .HasForeignKey(d => d.PropietarioId)
                    .HasConstraintName("FK__Vehiculo__Propie__2B3F6F97");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
