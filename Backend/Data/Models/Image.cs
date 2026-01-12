using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Image")]
public partial class Image
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? Url { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Type { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    [InverseProperty("Image")]
    public virtual ICollection<BrandBanner> BrandBanners { get; set; } = new List<BrandBanner>();

    [InverseProperty("Image")]
    public virtual ICollection<ProductDetailImage> ProductDetailImages { get; set; } = new List<ProductDetailImage>();

    [InverseProperty("Image")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
