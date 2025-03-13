using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data
{
    public class MobileNetworkRepository: Repository<MobileNetwork>, IMobileNetworkRepository
    {
        private readonly StoreContext _context;
        public MobileNetworkRepository(StoreContext context) : base(context)
        {
             _context = context;

        }

        public bool IsExists(int id)
        {
           return _context.MobileNetworks.Any(x => x.Id == id);        }

        public void Update(MobileNetwork mobileNetwork)
        {
           _context.MobileNetworks.Update(mobileNetwork);
        }
    }
}