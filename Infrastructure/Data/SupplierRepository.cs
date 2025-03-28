using System;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data;

public class SupplierRepository : Repository<Supplier>, ISupplierRepository
{
    private readonly StoreContext _context;
    public SupplierRepository(StoreContext context) : base(context)
    {
        _context = context;
    }

    public bool IsExists(int id)
    {
        return _context.Suppliers.Any(x => x.Id == id);     
    }

    public void Update(Supplier supplier)
    {
        _context.Suppliers.Update(supplier);
    }
}
