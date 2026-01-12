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
    public string Url { get; set; } = null!;

    [InverseProperty("Image")]
    public virtual ICollection<ProductVariantImage> ProductVariantImages { get; set; } = new List<ProductVariantImage>();
}
