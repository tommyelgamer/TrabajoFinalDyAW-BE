﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TrabajoFinalDyAW.Models;

public partial class TrabajoFinalContext : DbContext
{
    public TrabajoFinalContext(DbContextOptions<TrabajoFinalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Merchandise> Merchandise { get; set; }

    public virtual DbSet<Permissionclaim> Permissionclaim { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtoken { get; set; }

    public virtual DbSet<User> User { get; set; }

    public virtual DbSet<Userpermisssionclaim> Userpermisssionclaim { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Merchandise>(entity =>
        {
            entity.ToTable("merchandise");

            entity.HasIndex(e => e.MerchandiseBarcode, "IX_merchandise").IsUnique();

            entity.Property(e => e.MerchandiseId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("merchandise_id");
            entity.Property(e => e.MerchandiseBarcode)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("merchandise_barcode");
            entity.Property(e => e.MerchandiseDescription)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("merchandise_description");
            entity.Property(e => e.MerchandiseName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("merchandise_name");
            entity.Property(e => e.MerchandiseStock).HasColumnName("merchandise_stock");
        });

        modelBuilder.Entity<Permissionclaim>(entity =>
        {
            entity.HasKey(e => e.PermissionclaimName);

            entity.ToTable("permissionclaim");

            entity.Property(e => e.PermissionclaimName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("permissionclaim_name");
        });

        modelBuilder.Entity<Refreshtoken>(entity =>
        {
            entity.ToTable("refreshtoken");

            entity.Property(e => e.RefreshtokenId)
                .ValueGeneratedNever()
                .HasColumnName("refreshtoken_id");
            entity.Property(e => e.RefreshtokenExpire)
                .HasColumnType("datetime")
                .HasColumnName("refreshtoken_expire");
            entity.Property(e => e.RefreshtokenValue)
                .IsUnicode(false)
                .HasColumnName("refreshtoken_value");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Refreshtoken)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_refreshtoken_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.HasIndex(e => e.UserUsername, "IX_user").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("user_id");
            entity.Property(e => e.UserPassword)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_password");
            entity.Property(e => e.UserUsername)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("user_username");
        });

        modelBuilder.Entity<Userpermisssionclaim>(entity =>
        {
            entity.HasKey(e => e.UserpermissionclaimId);

            entity.ToTable("userpermisssionclaim");

            entity.Property(e => e.UserpermissionclaimId)
                .ValueGeneratedNever()
                .HasColumnName("userpermissionclaim_id");
            entity.Property(e => e.PermissionclaimName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("permissionclaim_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.PermissionclaimNameNavigation).WithMany(p => p.Userpermisssionclaim)
                .HasForeignKey(d => d.PermissionclaimName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userpermisssionclaim_permissionclaim");

            entity.HasOne(d => d.User).WithMany(p => p.Userpermisssionclaim)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_userpermisssionclaim_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}