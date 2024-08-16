using System;
using System.Collections.Generic;
using LECCIONCLIENTES.Models;
using Microsoft.EntityFrameworkCore;

namespace LECCIONCLIENTES.Context;

public partial class ClientesEfContext : DbContext
{
    public ClientesEfContext()
    {
    }

    public ClientesEfContext(DbContextOptions<ClientesEfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=PC13_LAB1\\SQLEXPRESS; Initial Catalog=ClientesEF; Integrated Security=True; Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Idc).HasName("PK__CLIENTES__C4971C2B6FFEC696");

            entity.ToTable("CLIENTES");

            entity.Property(e => e.Idc)
                .ValueGeneratedNever()
                .HasColumnName("IDC");
            entity.Property(e => e.Apellido)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("APELLIDO");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("FECHA_CREACION");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Ids).HasName("PK__SERVICIO__C4971C3B6D69BB06");

            entity.ToTable("SERVICIOS");

            entity.Property(e => e.Ids)
                .ValueGeneratedNever()
                .HasColumnName("IDS");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("FECHA_CREACION");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
