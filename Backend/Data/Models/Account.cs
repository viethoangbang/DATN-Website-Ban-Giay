using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Account")]
[Index("Email", Name = "IX_Account_Email")]
[Index("Status", Name = "IX_Account_Status")]
[Index("Email", Name = "UQ_Account_Email", IsUnique = true)]
public partial class Account
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [StringLength(255)]
    public string FullName { get; set; } = null!;

    [StringLength(1000)]
    [Unicode(false)]
    public string? AvatarUrl { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();

    [InverseProperty("Account")]
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    [InverseProperty("Account")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
