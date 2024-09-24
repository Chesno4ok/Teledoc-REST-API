using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Teledoc_REST_API.Models;

namespace Teledoc_REST_API;

public partial class TeledocContext : DbContext
{
    public TeledocContext()
    {
    }

    public TeledocContext(DbContextOptions<TeledocContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuthorizationRight> AuthorizationRights { get; set; }

    public virtual DbSet<AuthorizationToken> AuthorizationTokens { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientType> ClientTypes { get; set; }

    public virtual DbSet<Founder> Founders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#error Вставьте сюда строку подключения
        => optionsBuilder.UseNpgsql("[CONNECTION STRING HERE]");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorizationRight>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AuthorizationRights_pkey");
        });

        modelBuilder.Entity<AuthorizationToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AuthorizationTokens_pkey");

            entity.HasOne(d => d.AuthorizationRightNavigation).WithMany(p => p.AuthorizationTokens)
                .HasForeignKey(d => d.AuthorizationRight)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AuthorizationRights_FK");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Clients_pkey");

            entity.Property(e => e.Inn).HasColumnName("INN");

            entity.HasOne(d => d.ClientTypeNavigation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.ClientType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ClientTypes_FK");
        });

        modelBuilder.Entity<ClientType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ClientTypes_pkey");
        });

        modelBuilder.Entity<Founder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Founders_pkey");

            entity.HasOne(d => d.Client).WithMany(p => p.Founders)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("Client_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
