using System;
using Core.Entities;

namespace Core.Interface;

public interface IItemRepository:IRepository<Item>
{
    void Update(Item product);
    bool IsExists(int id);
}
