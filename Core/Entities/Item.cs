using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Item : BaseEntity
    {
        [Required]
        public string Title { get; set; }  = string.Empty;  
        [Required]
        public string Description { get; set; }  = string.Empty;
        public int? BrandId { get; set; }
        public int? ModelId { get; set; }

        [Required]
        public double Price { get; set; }

        public int Stock { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string EMI { get; set; }  = string.Empty; // Unique EMI identifier
        public bool IsSingle   { get; set; }

        public string ImageUrl { get; set; }  = string.Empty;

        public string Color { get; set; }  = string.Empty;
        public string Condition { get; set; }  = string.Empty;
        public int? ItemTypeId { get; set; }
        [ForeignKey("ItemTypeId")]
        public ItemType? ItemType { get; set; }

        // Navigation Properties
        [ForeignKey("BrandId")]
        public virtual Brand? Brand { get; set; }

        [ForeignKey("ModelId")]
        public virtual Model? Model { get; set; }

        public int? LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; }

        public int? MobileNetworkId { get; set; }
        [ForeignKey("MobileNetworkId")]
        public virtual MobileNetwork? MobileNetwork { get; set; }

        public int? StorageId { get; set; }
        [ForeignKey("StorageId")]
        public virtual Storage? Storage { get; set; }
        [ForeignKey("SupplierId")]
        public int? SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public ICollection<Core.Entities.Image> Images { get; set; } = new List<Core.Entities.Image>();
    }
}