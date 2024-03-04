using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Core.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [Range(1,100)]
        public int DisplayOrder { get; set; }
    }
}