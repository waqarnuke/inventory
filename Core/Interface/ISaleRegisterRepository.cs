using System;
using Core.Entities;

namespace Core.Interface;

public interface ISaleRegisterRepository : IRepository<SaleRegister>
{
     void Update(SaleRegister sellRegister);
}   
