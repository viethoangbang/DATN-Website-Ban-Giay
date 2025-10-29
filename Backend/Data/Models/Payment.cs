using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    [Table("payments")]
    public class Payment
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("payment_date")]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column("payment_method")]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = string.Empty; // COD, bank_transfer, vnpay, momo

        [Required]
        [Column("status")]
        [MaxLength(50)]
        public string Status { get; set; } = "pending"; // pending, completed, failed

        [Column("transaction_id")]
        [MaxLength(255)]
        public string? TransactionId { get; set; }

        [Required]
        [Column("order_id")]
        public string OrderId { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
    }
}

