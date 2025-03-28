using System;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class ImageService : IImageService
{
    public readonly Cloudinary _cloudinary;
    public ImageService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(account);
    }

    public async Task<ImageUploadResult> AddPhoto(IFormFile file)
    {
        if(file.Length > 0)
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                //File = new FileDescription(@"https://cloudinary-devs.github.io/cld-docs-assets/assets/images/cld-sample.jpg"),
                File = new FileDescription(file.FileName,stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill")
            };
            
            var uploadResult = await _cloudinary .UploadAsync(uploadParams);

            if(uploadResult.Error !=null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return uploadResult;
        }

        return null;
    }

    public async Task<DeletionResult> DeletePhoto(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        return await _cloudinary.DestroyAsync(deleteParams);
    }
}
