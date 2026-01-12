using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Cart")]
[Index("AccountId", Name = "IX_Cart_AccountID")]
public partial class Cart
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Carts")]
    public virtual Account Account { get; set; } = null!;

    [InverseProperty("Cart")]
    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
}
