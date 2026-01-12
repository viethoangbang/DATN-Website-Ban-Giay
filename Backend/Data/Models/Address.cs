using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Address")]
public partial class Address
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    public string? ProvinceName { get; set; }

    [StringLength(100)]
    public string? DistrictName { get; set; }

    [StringLength(100)]
    public string? WardName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    [StringLength(100)]
    public string? ReceiverName { get; set; }

    [StringLength(30)]
    public string? ReceiverPhone { get; set; }

    [StringLength(100)]
    public string? ReceiverEmail { get; set; }

    [Column("AccountID")]
    public int? AccountId { get; set; }

    public int? ProvinceId { get; set; }

    public int? DistrictId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? WardCode { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("Addresses")]
    public virtual Account? Account { get; set; }

    [InverseProperty("AddressDelivery")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
