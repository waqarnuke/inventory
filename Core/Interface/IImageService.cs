using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Core.Interface;

public interface IImageService
{
    Task<ImageUploadResult> AddPhoto(IFormFile file);
    Task<DeletionResult> DeletePhoto(string publicId);
}
