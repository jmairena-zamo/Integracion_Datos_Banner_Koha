using System;
using System.Collections.Generic;
using ApiBase.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace ApiBase.Context;

public partial class ZamoWebAppContext : DbContext
{
    public ZamoWebAppContext()
    {
    }

    public ZamoWebAppContext(DbContextOptions<ZamoWebAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Tbl_test_Estudiante> Tbl_test_Estudiantes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:ZamoWebAppConnectionString");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tbl_test_Estudiante>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tbl_test__3214EC070DF61EA6");

            entity.Property(e => e.Apellidos)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.CodigoEstado)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CodigoEstudiante)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreador).HasColumnType("datetime");
            entity.Property(e => e.FechaModifica).HasColumnType("datetime");
            entity.Property(e => e.Nombres)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioCreador)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioModifica)
                .HasMaxLength(128)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
