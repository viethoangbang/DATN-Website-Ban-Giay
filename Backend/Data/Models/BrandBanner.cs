using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("BrandBanner")]
[Index("Brand", Name = "IX_BrandBanner_Brand")]
[Index("Status", Name = "IX_BrandBanner_Status")]
public partial class BrandBanner
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    public string Brand { get; set; } = null!; // Keep for backward compatibility

    [Column("BrandID")]
    public int? BrandId { get; set; }

    [Column("ImageID")]
    public int ImageId { get; set; }

    public int? DisplayOrder { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [StringLength(100)]
    public string? CreateBy { get; set; }

    [StringLength(100)]
    public string? UpdateBy { get; set; }

    [ForeignKey("ImageId")]
    [InverseProperty("BrandBanners")]
    public virtual Image Image { get; set; } = null!;

    [ForeignKey("BrandId")]
    [InverseProperty("BrandBanners")]
    public virtual Brand? BrandNavigation { get; set; }
}
