using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    [Table("cart_items")]
    public class CartItem
    {
        [Key]
        [Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Column("cart_id")]
        public string CartId { get; set; } = string.Empty;

        [Required]
        [Column("product_variant_id")]
        public string ProductVariantId { get; set; } = string.Empty;

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; } = 1;

        // Navigation properties
        [ForeignKey("CartId")]
        public virtual Cart? Cart { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariant? ProductVariant { get; set; }
    }
}

