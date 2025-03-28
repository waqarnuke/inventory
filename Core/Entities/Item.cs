using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Item : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int? BrandId { get; set; }
        public int? ModelId { get; set; }

        [Required]
        public double Price { get; set; }

        public int Stock { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string EMI { get; set; } // Unique EMI identifier
        public bool IsSingle   { get; set; }

        public string ImageUrl { get; set; }

        public string Color { get; set; }
        public string Condition { get; set; }
        public int? ItemTypeId { get; set; }
        [ForeignKey("ItemTypeId")]
        public ItemType ItemType { get; set; }

        // Navigation Properties
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        [ForeignKey("ModelId")]
        public virtual Model Model { get; set; }

        public int? LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        public int? MobileNetworkId { get; set; }
        [ForeignKey("MobileNetworkId")]
        public virtual MobileNetwork MobileNetwork { get; set; }

        public int? StorageId { get; set; }
        [ForeignKey("StorageId")]
        public virtual Storage Storage { get; set; }
        [ForeignKey("SupplierId")]
        public int? SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public ICollection<Core.Entities.Image> Images { get; set; } = new List<Core.Entities.Image>();
    }
}