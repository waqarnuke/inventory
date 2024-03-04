using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly StoreContext _context;
        public CategoryController(StoreContext context)
        {   
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(u=> u.Id == id);
            return category;
        }
        
    }
}