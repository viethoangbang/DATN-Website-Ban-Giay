using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

[Table("Payment")]
[Index("OrderId", Name = "IDX_Payment_OrderID")]
[Index("TransactionNo", Name = "IDX_Payment_TransactionNo")]
public partial class Payment
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("OrderID")]
    public int OrderId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? TransactionNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BankCode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CardType { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PaymentMethod { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? Amount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? ResponseCode { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ResponseMessage { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? SecureHash { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PaidDate { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Payments")]
    public virtual Order Order { get; set; } = null!;
}
