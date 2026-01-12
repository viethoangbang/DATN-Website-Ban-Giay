using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("CartDetail")]
[Index("CartId", Name = "IX_CartDetail_CartID")]
[Index("ProductVariantId", Name = "IX_CartDetail_ProductVariantID")]
[Index("CartId", "ProductVariantId", Name = "UQ_CartDetail", IsUnique = true)]
public partial class CartDetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("CartID")]
    public int CartId { get; set; }

    [Column("ProductVariantID")]
    public int ProductVariantId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartDetails")]
    public virtual Cart Cart { get; set; } = null!;

    [ForeignKey("ProductVariantId")]
    [InverseProperty("CartDetails")]
    public virtual ProductVariant ProductVariant { get; set; } = null!;
}
