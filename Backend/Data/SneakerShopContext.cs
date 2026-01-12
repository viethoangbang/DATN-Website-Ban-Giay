using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace Data;

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

    public virtual DbSet<BrandBanner> BrandBanners { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartDetail> CartDetails { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDetail> ProductDetails { get; set; }

    public virtual DbSet<ProductDetailImage> ProductDetailImages { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC27240D830D");
        });

        modelBuilder.Entity<AccountRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountR__3214EC27FA5C381B");

            entity.HasOne(d => d.Account).WithMany(p => p.AccountRoles).HasConstraintName("FK__AccountRo__Accou__3E52440B");

            entity.HasOne(d => d.Role).WithMany(p => p.AccountRoles).HasConstraintName("FK__AccountRo__RoleI__3F466844");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Address__3214EC2732AEAD62");

            entity.HasOne(d => d.Account).WithMany(p => p.Addresses).HasConstraintName("FK__Address__Account__4222D4EF");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Brand");
        });

        modelBuilder.Entity<BrandBanner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BrandBan__3214EC2744D899AE");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.Status).HasDefaultValue("Active");

            entity.HasOne(d => d.Image).WithMany(p => p.BrandBanners).HasConstraintName("FK_BrandBanner_Image");
            entity.HasOne(d => d.BrandNavigation).WithMany(p => p.BrandBanners).HasConstraintName("FK_BrandBanner_Brand");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3214EC271F193365");

            entity.HasOne(d => d.Account).WithMany(p => p.Carts).HasConstraintName("FK__Cart__AccountID__5535A963");
        });

        modelBuilder.Entity<CartDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CartDeta__3214EC27B3794465");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartDetails).HasConstraintName("FK__CartDetai__CartI__5812160E");

            entity.HasOne(d => d.ProductDetail).WithMany(p => p.CartDetails).HasConstraintName("FK__CartDetai__Produ__59063A47");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC27EA89B713");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Color__3214EC274E709633");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3214EC2744886581");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Active");

            entity.HasOne(d => d.Product).WithMany(p => p.Discounts).HasConstraintName("FK_Discount_Product");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Image__3214EC27555A91A8");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC271E025CDC");

            entity.Property(e => e.ShippingFee).HasDefaultValue(0m);
            entity.Property(e => e.VoucherDiscount).HasDefaultValue(0m);

            entity.HasOne(d => d.AddressDelivery).WithMany(p => p.Orders).HasConstraintName("FK__Order__AddressDe__5DCAEF64");

            entity.HasOne(d => d.Customer).WithMany(p => p.OrderCustomers).HasConstraintName("FK__Order__CustomerI__5BE2A6F2");

            entity.HasOne(d => d.Employee).WithMany(p => p.OrderEmployees).HasConstraintName("FK__Order__EmployeeI__5CD6CB2B");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC2734AD78DD");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).HasConstraintName("FK__OrderDeta__Order__619B8048");

            entity.HasOne(d => d.ProductDetail).WithMany(p => p.OrderDetails).HasConstraintName("FK__OrderDeta__Produ__60A75C0F");
        });

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderSta__3214EC273EAC1B26");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderStatusHistories).HasConstraintName("FK__OrderStat__Order__0C85DE4D");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC272F161BC2");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments).HasConstraintName("FK__Payment__OrderID__02FC7413");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC27C28975DA");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Weight).HasDefaultValue(1000);

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK__Product__Categor__46E78A0C");
            entity.HasOne(d => d.BrandNavigation).WithMany(p => p.Products).HasConstraintName("FK_Product_Brand");
        });

        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductD__3214EC2759E29E4C");

            entity.HasOne(d => d.Color).WithMany(p => p.ProductDetails).HasConstraintName("FK__ProductDe__Color__5070F446");

            entity.HasOne(d => d.Image).WithMany(p => p.ProductDetails).HasConstraintName("FK__ProductDe__Image__4F7CD00D");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDetails).HasConstraintName("FK__ProductDe__Produ__52593CB8");

            entity.HasOne(d => d.Size).WithMany(p => p.ProductDetails).HasConstraintName("FK__ProductDe__SizeI__5165187F");
        });

        modelBuilder.Entity<ProductDetailImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductD__3214EC27A4E3FDFE");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);

            entity.HasOne(d => d.Image).WithMany(p => p.ProductDetailImages).HasConstraintName("FK__ProductDe__Image__6B24EA82");

            entity.HasOne(d => d.ProductDetail).WithMany(p => p.ProductDetailImages).HasConstraintName("FK__ProductDe__Produ__6A30C649");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC271958A971");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shipment__3214EC27E9CB7BFA");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.InsuranceFee).HasDefaultValue(0m);

            entity.HasOne(d => d.Order).WithMany(p => p.Shipments).HasConstraintName("FK__Shipment__OrderI__08B54D69");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Size__3214EC271C999E6C");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC2799D75436");

            entity.HasOne(d => d.Category).WithMany(p => p.Vouchers).HasConstraintName("FK__Voucher__Categor__6E01572D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
