using Microsoft.AspNetCore.Http;
using API.Helper;

namespace API.Dtos
{
    public class ProductPhotoDto
    {
        [MaxFileSize (2 * 1024 * 1024)]
        [AllowedExtensions(new[] {".jpg", ".png", ".jpeg"})]
        public IFormFile file { get; set; }
    }
}