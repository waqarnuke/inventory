using System;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data;

public class SaleRegisterRepository : Repository<SaleRegister>, ISaleRegisterRepository
{
    private readonly StoreContext _context;
    public SaleRegisterRepository(StoreContext context) : base(context)
    {
        _context = context;
    }

    public void Update(SaleRegister register)
    {
        _context.SaleRegisters.Update(register);
    }

}
