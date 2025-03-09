using System;
using Core.Entities;

namespace Core.Interface;

public interface IRegisterRepository : IRepository<Register>
{
    void Update(Register register);
}   
