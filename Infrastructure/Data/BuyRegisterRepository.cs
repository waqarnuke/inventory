using System;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data;

public class BuyRegisterRepository : Repository<BuyRegister>, IBuyRegisterRepository
{
    private readonly StoreContext _context;
    public BuyRegisterRepository(StoreContext context) : base(context)
    {
        _context = context;
    }

    public void Update(BuyRegister register)
    {
        _context.BuyRegisters.Update(register);
    }
}
