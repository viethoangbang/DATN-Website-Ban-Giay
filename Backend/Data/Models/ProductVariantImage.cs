using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("ProductVariantImage")]
[Index("ImageId", Name = "IX_ProductVariantImage_ImageID")]
[Index("IsMain", Name = "IX_ProductVariantImage_IsMain")]
[Index("ProductVariantId", Name = "IX_ProductVariantImage_ProductVariantID")]
[Index("ProductVariantId", "ImageId", Name = "UQ_ProductVariantImage", IsUnique = true)]
public partial class ProductVariantImage
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ProductVariantID")]
    public int ProductVariantId { get; set; }

    [Column("ImageID")]
    public int ImageId { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsMain { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [ForeignKey("ImageId")]
    [InverseProperty("ProductVariantImages")]
    public virtual Image Image { get; set; } = null!;

    [ForeignKey("ProductVariantId")]
    [InverseProperty("ProductVariantImages")]
    public virtual ProductVariant ProductVariant { get; set; } = null!;
}
