using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("CartDetail")]
public partial class CartDetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int? Quantity { get; set; }

    [Column("CartID")]
    public int? CartId { get; set; }

    [Column("ProductDetailID")]
    public int? ProductDetailId { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartDetails")]
    public virtual Cart? Cart { get; set; }

    [ForeignKey("ProductDetailId")]
    [InverseProperty("CartDetails")]
    public virtual ProductDetail? ProductDetail { get; set; }
}
