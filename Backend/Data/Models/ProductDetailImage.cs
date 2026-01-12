using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("ProductDetailImage")]
[Index("ImageId", Name = "IX_ProductDetailImage_ImageID")]
[Index("ProductDetailId", Name = "IX_ProductDetailImage_ProductDetailID")]
[Index("ProductDetailId", "ImageId", Name = "UQ__ProductD__EBDCB9DB98A1F9A8", IsUnique = true)]
public partial class ProductDetailImage
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ProductDetailID")]
    public int ProductDetailId { get; set; }

    [Column("ImageID")]
    public int ImageId { get; set; }

    public int? DisplayOrder { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [ForeignKey("ImageId")]
    [InverseProperty("ProductDetailImages")]
    public virtual Image Image { get; set; } = null!;

    [ForeignKey("ProductDetailId")]
    [InverseProperty("ProductDetailImages")]
    public virtual ProductDetail ProductDetail { get; set; } = null!;
}
