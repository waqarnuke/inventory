using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Brand
    {
        [Key]
        public int Id { get; set; } 
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } = string.Empty;

    }
}