using System;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data;

public class RegisterRepository : Repository<Register>,  IRegisterRepository
{
    private readonly StoreContext _context;
    public RegisterRepository(StoreContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Register register)
    {
        _context.Registers.Update(register);
    }
}
