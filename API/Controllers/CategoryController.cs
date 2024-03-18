using API.Errors;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoryController  : BaseApiController 
    {
        private readonly IUnitOfWork _repo;
        public CategoryController(IUnitOfWork repo)
        {   
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var allrow = await _repo.categoryRepository.ListAllAsync();
            return Ok(allrow);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            //var category = _repo.categoryRepository.Get(u => u.Id == id);
            var category = await _repo.categoryRepository.GetByIdAsync(id);

            if(category == null) return NotFound(new ApiResponse(404));

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Category category)
        {
            var item =  _repo.categoryRepository.Get(u => u.Id == category.Id); 
            if(ModelState.IsValid)
            {
                _repo.categoryRepository.Add(category);
                var result =  await _repo.Save();
                return Ok();
            }
            return NotFound();
        }
        [HttpPut]
        public async Task<ActionResult<Category>> Update(Category category)
        {
            Category cat = await _repo.categoryRepository
            .Get(u=>u.Id == category.Id);
            cat.Name=category.Name;
            cat.DisplayOrder = category.DisplayOrder;
            if(cat != null)
            {
                _repo.categoryRepository.Update(cat);
                var result = await _repo.Save();
                return Ok(cat);
            } 
            return NotFound();
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            Category category  = await _repo.categoryRepository.GetByIdAsync(id);
            if(category != null)
            {
                _repo.categoryRepository.Remove(category);
                _repo.Save();
                //return Ok();
            } 
            //return NotFound();
        }
        
    }
}