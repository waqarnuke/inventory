using System;
using Core.Entities;

namespace Core.Interface;

public interface IBuyRegisterRepository : IRepository<BuyRegister>
{
    void Update(BuyRegister register);
}
