using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    [Table("product_variants")]
    public class ProductVariant
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Column("product_id")]
        public string ProductId { get; set; } = string.Empty;

        [Column("color")]
        [MaxLength(50)]
        public string? Color { get; set; }

        [Column("size")]
        [MaxLength(20)]
        public string? Size { get; set; }

        [Required]
        [Column("sku")]
        [MaxLength(100)]
        public string Sku { get; set; } = string.Empty;

        [Column("stock")]
        public int Stock { get; set; } = 0;

        // Navigation properties
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

