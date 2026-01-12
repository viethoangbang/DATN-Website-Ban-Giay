using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Role")]
[Index("Name", Name = "UQ_Role_Name", IsUnique = true)]
public partial class Role
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();
}
