using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Shipment")]
[Index("OrderId", Name = "IDX_Shipment_OrderID")]
[Index("ShipmentCode", Name = "IDX_Shipment_ShipmentCode")]
public partial class Shipment
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("OrderID")]
    public int OrderId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ShipmentCode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PartnerCode { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? PickAddress { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PickProvince { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PickDistrict { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PickWard { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? PickTel { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PickName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? DeliveryAddress { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? DeliveryProvince { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? DeliveryDistrict { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? DeliveryWard { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? DeliveryTel { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? DeliveryName { get; set; }

    public int? Weight { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? Value { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? ShippingFee { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? InsuranceFee { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? StatusText { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PickedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeliveredDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EstimatedDeliveryDate { get; set; }

    [Column(TypeName = "text")]
    public string? Note { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Shipments")]
    public virtual Order Order { get; set; } = null!;
}
