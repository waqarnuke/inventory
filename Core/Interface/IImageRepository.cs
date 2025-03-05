using System;
using Core.Entities;

namespace Core.Interface;

public interface IImageRepository : IRepository<Image>
{
    void Update(Image product);
}
