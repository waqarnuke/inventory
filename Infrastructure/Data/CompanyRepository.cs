using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data
{
    public class CompanyRepository :Repository<Company>, ICompanyRepository
    {
        private readonly StoreContext _context;
        public CompanyRepository(StoreContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Company company)
        {
            _context.Companies.Update(company);
        }

        public bool IsExists(int id)
        {
            return _context.Companies.Any(x => x.Id == id);
        }
    }
}