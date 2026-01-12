using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("ProductDetail")]
public partial class ProductDetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    [Column("ImageID")]
    public int? ImageId { get; set; }

    [Column("ColorID")]
    public int? ColorId { get; set; }

    [Column("SizeID")]
    public int? SizeId { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [InverseProperty("ProductDetail")]
    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    [ForeignKey("ColorId")]
    [InverseProperty("ProductDetails")]
    public virtual Color? Color { get; set; }

    [ForeignKey("ImageId")]
    [InverseProperty("ProductDetails")]
    public virtual Image? Image { get; set; }

    [InverseProperty("ProductDetail")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("ProductId")]
    [InverseProperty("ProductDetails")]
    public virtual Product? Product { get; set; }

    [InverseProperty("ProductDetail")]
    public virtual ICollection<ProductDetailImage> ProductDetailImages { get; set; } = new List<ProductDetailImage>();

    [ForeignKey("SizeId")]
    [InverseProperty("ProductDetails")]
    public virtual Size? Size { get; set; }
}
