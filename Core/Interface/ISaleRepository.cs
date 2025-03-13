using System;
using Core.Entities;

namespace Core.Interface;

public interface ISaleRepository : IRepository<Sale>
{
    void Update(Sale sele);
}
