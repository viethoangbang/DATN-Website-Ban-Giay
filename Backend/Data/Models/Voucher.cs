using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Voucher")]
public partial class Voucher
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Code { get; set; }

    [StringLength(255)]
    public string? Name { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Type { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? Discount { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? MaxDiscount { get; set; }

    public int? Quantity { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? CreateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? UpdateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [Column("CategoryID")]
    public int? CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Vouchers")]
    public virtual Category? Category { get; set; }
}
