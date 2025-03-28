using System;
using Core.Entities;

namespace Core.Interface;

public interface ISupplierRepository : IRepository<Supplier>
{
    void Update(Supplier supplier);
    bool IsExists(int id);
}
