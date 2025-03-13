using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interface
{
    public interface IStorageRepository :IRepository<Storage>
{
    void Update(Storage storage);
    bool IsExists(int id);
}
}