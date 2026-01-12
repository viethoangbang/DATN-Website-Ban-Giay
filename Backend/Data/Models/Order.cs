using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Order")]
public partial class Order
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? Total { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? CreateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeliveryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? ProductDiscount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    [Column("CustomerID")]
    public int? CustomerId { get; set; }

    [Column("EmployeeID")]
    public int? EmployeeId { get; set; }

    [Column("AddressDeliveryID")]
    public int? AddressDeliveryId { get; set; }

    [StringLength(100)]
    public string? ReceiverName { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? ReceiverPhone { get; set; }

    [StringLength(255)]
    public string? ReceiverAddress { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? ProductTotal { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Code { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? ShippingFee { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? VoucherCode { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? VoucherDiscount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PaymentMethod { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PaymentStatus { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ShippingStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EstimatedDeliveryDate { get; set; }

    public int? ActualWeight { get; set; }

    [Column(TypeName = "text")]
    public string? Note { get; set; }

    [ForeignKey("AddressDeliveryId")]
    [InverseProperty("Orders")]
    public virtual Address? AddressDelivery { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("OrderCustomers")]
    public virtual Account? Customer { get; set; }

    [ForeignKey("EmployeeId")]
    [InverseProperty("OrderEmployees")]
    public virtual Account? Employee { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; } = new List<OrderStatusHistory>();

    [InverseProperty("Order")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [InverseProperty("Order")]
    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
