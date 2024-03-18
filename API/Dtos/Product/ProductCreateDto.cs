using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ProductCreateDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }    
        [Required]    
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        [RegularExpression(@"^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$", 
            ErrorMessage = "Price must be a decimal (e.g 20.30)")]
        public double ListPrice { get; set; }
        
        [Required]
        [RegularExpression(@"^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$", 
            ErrorMessage = "Price must be a decimal (e.g 20.30)")]
        public double Price { get; set; }
        [RegularExpression(@"^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$", 
            ErrorMessage = "Price must be a decimal (e.g 20.30)")]
        public double Price50 { get; set; }
        [RegularExpression(@"^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$", 
            ErrorMessage = "Price must be a decimal (e.g 20.30)")]
        public double Price100 { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}