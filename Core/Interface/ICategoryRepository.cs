using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interface
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}