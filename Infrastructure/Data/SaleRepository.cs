using System;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data;

public class SaleRepository : Repository<Sale>, ISaleRepository
{
    private readonly StoreContext _context;
    public SaleRepository(StoreContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Sale sale)
    {
        _context.Sales.Update(sale);
    }
}
