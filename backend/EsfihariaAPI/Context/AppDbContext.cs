using System;
using System.Collections.Generic;
using EsfihariaAPI.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace EsfihariaAPI.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Addresses> Addresses { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Ordercategory> Ordercategories { get; set; }

    public virtual DbSet<Orderproduct> Orderproducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productcategory> Productcategories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=esfiha_bd;uid=root;pwd=senai2024", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.42-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Addresses>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("addresses");

            entity.HasIndex(e => e.IdUser, "IdUser_idx");

            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .HasColumnName("Address");
            entity.Property(e => e.Cep).HasColumnName("CEP");
            entity.Property(e => e.Complement).HasMaxLength(100);
            entity.Property(e => e.Landmark).HasMaxLength(45);
            entity.Property(e => e.Neighborhood).HasMaxLength(45);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("IdUser");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("driver");

            entity.HasIndex(e => e.IdStatus, "IdxDriverIdStatus");

            entity.HasIndex(e => e.IdUser, "IdxDriverIdUser");

            entity.Property(e => e.Cnh)
                .HasMaxLength(90)
                .HasColumnName("CNH");
            entity.Property(e => e.LicensePlate).HasMaxLength(45);

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkDriverStatus");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkDriverUser");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.IdAddress, "IdxIdAddress");

            entity.HasIndex(e => e.IdOrderCategory, "IdxIdOrderCategory");

            entity.HasIndex(e => e.IdStatus, "IdxIdStatus");

            entity.HasIndex(e => e.IdUser, "IdxIdUser");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DeliveryTime).HasPrecision(5, 2);
            entity.Property(e => e.DeliveryValue).HasPrecision(10, 2);
            entity.Property(e => e.DiscountValue).HasPrecision(10, 2);
            entity.Property(e => e.Note).HasMaxLength(100);
            entity.Property(e => e.SubtotalValue).HasPrecision(10, 2);
            entity.Property(e => e.TotalValue).HasPrecision(10, 2);

            entity.HasOne(d => d.IdAddressNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdAddress)
                .HasConstraintName("FkOrderAddress");

            entity.HasOne(d => d.IdOrderCategoryNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdOrderCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkOrderOrderCategory");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkOrderStatus");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkOrderUser");
        });

        modelBuilder.Entity<Ordercategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ordercategory");

            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<Orderproduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orderproduct");

            entity.HasIndex(e => e.IdOrder, "IdOrder_idx");

            entity.HasIndex(e => e.IdProduct, "IdProduct_idx");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("IdOrder");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("IdProduct");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.IdCategory, "IdCategory_idx");

            entity.HasIndex(e => e.IdStatus, "IdStatus_idx");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Image).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Price).HasPrecision(10);

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("IdCategory");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("IdStatus");
        });

        modelBuilder.Entity<Productcategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("productcategory");

            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("status");

            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("transactions");

            entity.HasIndex(e => e.IdOrder, "IdOrder_idx");

            entity.HasIndex(e => e.IdStatus, "IdStatus_idx");

            entity.HasIndex(e => e.IdUser, "IdUser_idx");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.PayloadResponse).HasColumnType("json");
            entity.Property(e => e.TotalValue).HasPrecision(10);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkIdOrder");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkIdStatus");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkIdUser");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.IdRole, "IdRole_idx");

            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(90);
            entity.Property(e => e.Password).HasMaxLength(45);
            entity.Property(e => e.Phone).HasMaxLength(45);

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("IdRole");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
