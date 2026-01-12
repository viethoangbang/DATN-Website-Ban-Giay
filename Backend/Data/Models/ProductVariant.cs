using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("ProductVariant")]
[Index("ProductId", Name = "IX_ProductVariant_ProductID")]
[Index("Status", Name = "IX_ProductVariant_Status")]
[Index("ProductId", "Color", "Size", Name = "UQ_ProductVariant", IsUnique = true)]
public partial class ProductVariant
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ProductID")]
    public int ProductId { get; set; }

    [StringLength(50)]
    public string Color { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string Size { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [InverseProperty("ProductVariant")]
    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    [InverseProperty("ProductVariant")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("ProductId")]
    [InverseProperty("ProductVariants")]
    public virtual Product Product { get; set; } = null!;

    [InverseProperty("ProductVariant")]
    public virtual ICollection<ProductVariantImage> ProductVariantImages { get; set; } = new List<ProductVariantImage>();
}
