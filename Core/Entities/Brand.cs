using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Brand
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

    }
}