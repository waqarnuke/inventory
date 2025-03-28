using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Dtos.Item;

public class ItemCreateDto
{
        int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int BrandId { get; set; }

        public int ModelId { get; set; }

        [Required]
        public double Price { get; set; }

        public int Stock { get; set; }

        public string EMI { get; set; } // Unique EMI identifier

        public bool IsSingle { get; set; }

        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public string Condition { get; set; }
        public int ItemTypeId { get; set; } 
        public int LocationId { get; set; }
        public int MobileNetworkId { get; set; }
        public int StorageId { get; set; } 
        public int SupplierId { get; set; }            
}
