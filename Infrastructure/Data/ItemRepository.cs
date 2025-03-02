using System;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data;

public class ItemRepository : Repository<Item>, IItemRepository
{
    private readonly StoreContext _context;
    public ItemRepository(StoreContext context) : base(context)
    {
        _context = context;
    }

    public bool IsExists(int id)
    {
        return _context.Items.Any(x => x.Id == id);
    }

    public void Update(Item item)
    {
        _context.Items.Update(item);
    }
}
