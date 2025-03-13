using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data
{
    public class LocationRepository: Repository<Location>, ILocationRepository
    {
        private readonly StoreContext _context;
        public LocationRepository(StoreContext context) : base(context)
        {
             _context = context;

        }

        public bool IsExists(int id)
        {
           return _context.Locations.Any(x => x.Id == id);        }

        public void Update(Location location)
        {
           _context.Locations.Update(location);
        }

        
    }
}