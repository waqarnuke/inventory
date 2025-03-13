using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interface
{
    public interface IBrandRepository :IRepository<Brand>
{
    void Update(Brand brand);
    bool IsExists(int id);
}
}