using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data
{
    public class ModelRepository: Repository<Model>, IModelRepository
    {
        private readonly StoreContext _context;
        public ModelRepository(StoreContext context) : base(context)
        {
             _context = context;

        }

        public bool IsExists(int id)
        {
           return _context.Models.Any(x => x.Id == id);        }

        public void Update(Model model)
        {
           _context.Models.Update(model);
        }

       
    }
}