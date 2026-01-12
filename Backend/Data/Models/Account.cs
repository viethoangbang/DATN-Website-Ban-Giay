using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Account")]
[Index("Email", Name = "UQ__Account__A9D1053449DC465B", IsUnique = true)]
public partial class Account
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Password { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [StringLength(255)]
    public string? FullName { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? Sex { get; set; }

    public int? BirthYear { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? CreateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? UpdateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [Column("Avatar_Url")]
    [StringLength(1000)]
    [Unicode(false)]
    public string? AvatarUrl { get; set; }

    [InverseProperty("Account")]
    public virtual ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();

    [InverseProperty("Account")]
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    [InverseProperty("Account")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [InverseProperty("Customer")]
    public virtual ICollection<Order> OrderCustomers { get; set; } = new List<Order>();

    [InverseProperty("Employee")]
    public virtual ICollection<Order> OrderEmployees { get; set; } = new List<Order>();
}
