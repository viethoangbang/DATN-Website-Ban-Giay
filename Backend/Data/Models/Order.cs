using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("discount_id")]
        public string? DiscountId { get; set; }

        [Column("order_date")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("total_amount")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(50)]
        public string Status { get; set; } = "pending"; // pending, processing, shipped, delivered, cancelled

        [Column("payment_method")]
        [MaxLength(50)]
        public string? PaymentMethod { get; set; }

        [Column("shipping_address")]
        [MaxLength(500)]
        public string? ShippingAddress { get; set; }

        [Column("note")]
        [MaxLength(1000)]
        public string? Note { get; set; }

        [Required]
        [Column("user_id")]
        public string UserId { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("DiscountId")]
        public virtual Discount? Discount { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}

