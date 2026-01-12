using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Cart")]
public partial class Cart
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [Column("AccountID")]
    public int? AccountId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Carts")]
    public virtual Account? Account { get; set; }

    [InverseProperty("Cart")]
    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
}
