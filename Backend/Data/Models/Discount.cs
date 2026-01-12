using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Discount")]
[Index("StartDate", "EndDate", Name = "IX_Discount_Dates")]
[Index("ProductId", Name = "IX_Discount_ProductID")]
[Index("Status", Name = "IX_Discount_Status")]
public partial class Discount
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ProductID")]
    public int ProductId { get; set; }

    [StringLength(50)]
    public string DiscountType { get; set; } = null!;

    [Column(TypeName = "decimal(15, 2)")]
    public decimal DiscountValue { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [StringLength(100)]
    public string? CreateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [StringLength(100)]
    public string? UpdateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Discounts")]
    public virtual Product Product { get; set; } = null!;
}
