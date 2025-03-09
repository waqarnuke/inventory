using System;
using Core.Entities;

namespace Core.Interface;

public interface IBuyingRepository :IRepository<Buying>
{
    void Update(Buying buying);
}
