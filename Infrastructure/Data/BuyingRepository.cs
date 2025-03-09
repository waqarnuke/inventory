using System;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data;

public class BuyingRepository : Repository<Buying>, IBuyingRepository
{
    private readonly StoreContext _context;
    public BuyingRepository(StoreContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Buying buying)
    {
        _context.Buyings.Update(buying);
    }
}
