using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    [Table("shipments")]
    public class Shipment
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Column("order_id")]
        public string OrderId { get; set; } = string.Empty;

        [Column("courier_name")]
        [MaxLength(255)]
        public string? CourierName { get; set; }

        [Column("tracking_number")]
        [MaxLength(255)]
        public string? TrackingNumber { get; set; }

        [Column("shipped_at")]
        public DateTime? ShippedAt { get; set; }

        [Column("delivered_at")]
        public DateTime? DeliveredAt { get; set; }

        [Required]
        [Column("status")]
        [MaxLength(50)]
        public string Status { get; set; } = "pending"; // pending, shipped, in_transit, delivered

        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
    }
}

