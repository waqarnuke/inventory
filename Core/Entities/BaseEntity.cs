using System.ComponentModel.DataAnnotations;
namespace Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; } 
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime UpdatedTime { get; set; } = DateTime.Now;
        public string CreatedUser { get; set; }
        public string UpdatedUser { get; set; }
        public bool Status { get; set; }  
    }
}