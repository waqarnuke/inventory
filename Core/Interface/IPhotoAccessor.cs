
using Core.Entities;
using Microsoft.AspNetCore.Http;
namespace Core.Interface
{
    public interface IPhotoAccessor
    {
        Task<Photo> AddPhoto(IFormFile file);  
        Task<string> DeletePhoto(string publicId);       
    }
}