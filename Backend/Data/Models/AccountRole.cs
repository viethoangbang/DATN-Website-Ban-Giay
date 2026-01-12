using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("AccountRole")]
public partial class AccountRole
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("AccountID")]
    public int? AccountId { get; set; }

    [Column("RoleID")]
    public int? RoleId { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("AccountRoles")]
    public virtual Account? Account { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("AccountRoles")]
    public virtual Role? Role { get; set; }
}
