using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("OrderDetail")]
[Index("OrderId", Name = "IX_OrderDetail_OrderID")]
public partial class OrderDetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("OrderID")]
    public int OrderId { get; set; }

    [Column("ProductVariantID")]
    public int ProductVariantId { get; set; }

    [StringLength(255)]
    public string ProductName { get; set; } = null!;

    [StringLength(50)]
    public string Color { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string Size { get; set; } = null!;

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductVariantId")]
    [InverseProperty("OrderDetails")]
    public virtual ProductVariant ProductVariant { get; set; } = null!;
}
