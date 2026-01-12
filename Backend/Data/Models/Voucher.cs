using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Voucher")]
[Index("CategoryId", Name = "IX_Voucher_CategoryID")]
[Index("Code", Name = "IX_Voucher_Code")]
[Index("StartDate", "EndDate", Name = "IX_Voucher_Dates")]
[Index("ProductId", Name = "IX_Voucher_ProductID")]
[Index("Status", Name = "IX_Voucher_Status")]
[Index("Code", Name = "UQ_Voucher_Code", IsUnique = true)]
public partial class Voucher
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? PercentDiscount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? FixedDiscount { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MinOrderAmount { get; set; }

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Vouchers")]
    public virtual Category? Category { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Vouchers")]
    public virtual Product? Product { get; set; }
}
