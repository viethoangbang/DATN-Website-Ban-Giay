using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public partial class SneakerShopContext : DbContext
{
    public SneakerShopContext(DbContextOptions<SneakerShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountRole> AccountRoles { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartDetail> CartDetails { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<ProductVariantImage> ProductVariantImages { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Active");
        });

        modelBuilder.Entity<AccountRole>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.AccountRoles).HasConstraintName("FK_AccountRole_Account");

            entity.HasOne(d => d.Role).WithMany(p => p.AccountRoles).HasConstraintName("FK_AccountRole_Role");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasOne(d => d.Account).WithMany(p => p.Addresses).HasConstraintName("FK_Address_Account");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Account).WithMany(p => p.Carts).HasConstraintName("FK_Cart_Account");
        });

        modelBuilder.Entity<CartDetail>(entity =>
        {
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.Cart).WithMany(p => p.CartDetails).HasConstraintName("FK_CartDetail_Cart");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.CartDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartDetail_ProductVariant");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PaymentStatus).HasDefaultValue("Unpaid");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_ProductVariant");
        });

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderStatusHistories).HasConstraintName("FK_OrderStatusHistory_Order");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Active");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Brand");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Active");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants).HasConstraintName("FK_ProductVariant_Product");
        });

        modelBuilder.Entity<ProductVariantImage>(entity =>
        {
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Image).WithMany(p => p.ProductVariantImages).HasConstraintName("FK_ProductVariantImage_Image");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.ProductVariantImages).HasConstraintName("FK_ProductVariantImage_ProductVariant");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Active");

            entity.HasOne(d => d.Category).WithMany(p => p.Vouchers).HasConstraintName("FK_Voucher_Category");

            entity.HasOne(d => d.Product).WithMany(p => p.Vouchers).HasConstraintName("FK_Voucher_Product");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
