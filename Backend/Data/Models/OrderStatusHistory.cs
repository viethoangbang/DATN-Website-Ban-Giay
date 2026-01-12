using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("OrderStatusHistory")]
[Index("OrderId", Name = "IDX_OrderStatusHistory_OrderID")]
public partial class OrderStatusHistory
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("OrderID")]
    public int OrderId { get; set; }

    [StringLength(50)]
    public string? FromStatus { get; set; }

    [StringLength(50)]
    public string? ToStatus { get; set; }

    [Column(TypeName = "text")]
    public string? Note { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderStatusHistories")]
    public virtual Order Order { get; set; } = null!;
}
