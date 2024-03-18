using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Product : BaseEntity
    { 
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Display(Name = "List Price")]
        [Range(1,1000)]
        public double ListPrice { get; set; }
        
        [Required]
        [Display(Name = "Price for 1-50+")]
        [Range(1,1000)]
        public double Price { get; set; }
        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1,1000)]
        public double Price50 { get; set; }
        
        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1,1000)]
        public double Price100 { get; set; }
        //[ValidateNever]
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        //[ValidateNever]
        public Category Category { get; set; }

        private readonly List<Photo> _Photos = new List<Photo>();
        public IReadOnlyList<Photo> Photos => _Photos.AsReadOnly();

        public void AddPhoto(string pictureUrl, string fileName, bool isMain = false)
        {
            var photo = new Photo
            {
                FileName = fileName,
                PictureUrl = pictureUrl
            };
            
            if (_Photos.Count == 0) photo.IsMain = true;
            
            _Photos.Add(photo);
        }

        public void RemovePhoto(int id)
        {
            var photo = _Photos.Find(x => x.Id == id);
            _Photos.Remove(photo);
        }

        public void SetMainPhoto(int id)
        {
            var currentMain = _Photos.SingleOrDefault(item => item.IsMain);
            foreach (var item in _Photos.Where(item => item.IsMain))
            {
                item.IsMain = false;
            }
            
            var photo = _Photos.Find(x => x.Id == id);
            if (photo != null)
            {
                photo.IsMain = true;
                if (currentMain != null) currentMain.IsMain = false;
            }
        }
    }
}