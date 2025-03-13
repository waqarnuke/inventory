using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interface
{
    public interface IModelRepository :IRepository<Model>
{
    void Update(Model model);
    bool IsExists(int id);
}
}