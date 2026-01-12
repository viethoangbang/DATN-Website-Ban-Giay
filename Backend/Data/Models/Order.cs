using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Order")]
[Index("CreateDate", Name = "IX_Order_CreateDate")]
[Index("CustomerId", Name = "IX_Order_CustomerID")]
[Index("Status", Name = "IX_Order_Status")]
[Index("Code", Name = "UQ_Order_Code", IsUnique = true)]
public partial class Order
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(100)]
    public string ReceiverName { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string ReceiverPhone { get; set; } = null!;

    [StringLength(500)]
    public string DeliveryAddress { get; set; } = null!;

    [StringLength(1000)]
    public string? Note { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string PaymentMethod { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string PaymentStatus { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal SubTotal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ShippingFee { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CompletedDate { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual Account Customer { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; } = new List<OrderStatusHistory>();
}
