using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Interface
{
    public interface IPhotoService
    {
        Task<Photo> SaveToDiskAsync(IFormFile photo);
        void DeleteFromDisk(Photo photo);
    }
}