using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("OrderDetail")]
public partial class OrderDetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int? Quantity { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ProductName { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? Price { get; set; }

    [Column("ProductDetailID")]
    public int? ProductDetailId { get; set; }

    [Column("OrderID")]
    public int? OrderId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? OriginalPrice { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DiscountType { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? DiscountValue { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? FinalPrice { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderDetails")]
    public virtual Order? Order { get; set; }

    [ForeignKey("ProductDetailId")]
    [InverseProperty("OrderDetails")]
    public virtual ProductDetail? ProductDetail { get; set; }
}
