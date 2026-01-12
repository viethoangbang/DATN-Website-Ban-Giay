using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Product")]
[Index("BrandId", Name = "IX_Product_BrandID")]
[Index("CategoryId", Name = "IX_Product_CategoryID")]
[Index("Status", Name = "IX_Product_Status")]
[Index("Code", Name = "UQ_Product_Code", IsUnique = true)]
public partial class Product
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(255)]
    public string Name { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column("BrandID")]
    public int BrandId { get; set; }

    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [ForeignKey("BrandId")]
    [InverseProperty("Products")]
    public virtual Brand Brand { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

    [InverseProperty("Product")]
    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
