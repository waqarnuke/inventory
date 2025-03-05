using System;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data;

public class ImageRepository : Repository<Image>, IImageRepository
{
    private readonly StoreContext _context;
    public ImageRepository(StoreContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Image image)
    {
        _context.Images.Update(image);
    }
}
