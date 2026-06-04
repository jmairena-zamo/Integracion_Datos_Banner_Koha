using System;
using System.Collections.Generic;
using Integracion_Datos_Banner_Koha.Models.DB.Biblioteca;
using Microsoft.EntityFrameworkCore;

namespace Integracion_Datos_Banner_Koha.Context;

public partial class BibliotecaContext : DbContext
{
    public BibliotecaContext()
    {
    }

    public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<koha_log> koha_logs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=BibliotecaConnectionString");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<koha_log>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__koha_log__3213E83FCE2E4A8B");

            entity.ToTable("koha_log");

            entity.Property(e => e.Codigo_Estudiante)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Correo_estudiante)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fecha_Creacion).HasColumnType("datetime");
            entity.Property(e => e.Fecha_Modifica).HasColumnType("datetime");
            entity.Property(e => e.Host)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Usuario_Crea)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Usuario_Modifica)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.banner_JSON).IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
