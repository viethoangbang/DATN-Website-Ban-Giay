using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Address")]
[Index("AccountId", Name = "IX_Address_AccountID")]
public partial class Address
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("AccountID")]
    public int AccountId { get; set; }

    [StringLength(100)]
    public string ReceiverName { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string ReceiverPhone { get; set; } = null!;

    [StringLength(100)]
    public string Province { get; set; } = null!;

    [StringLength(100)]
    public string District { get; set; } = null!;

    [StringLength(100)]
    public string Ward { get; set; } = null!;

    [StringLength(255)]
    public string StreetAddress { get; set; } = null!;

    public bool IsDefault { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Addresses")]
    public virtual Account Account { get; set; } = null!;
}
