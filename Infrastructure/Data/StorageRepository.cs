using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data
{
    public class StorageRepository: Repository<Storage>, IStorageRepository
    {
        private readonly StoreContext _context;
        public StorageRepository(StoreContext context) : base(context)
        {
             _context = context;

        }

        public bool IsExists(int id)
        {
           return _context.Storages.Any(x => x.Id == id);        }

        public void Update(Storage storage)
        {
           _context.Storages.Update(storage);
        }

        
    }
}