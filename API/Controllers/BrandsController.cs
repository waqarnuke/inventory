using API.Dtos.Item;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BrandsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BrandsController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Brand>>> GetBrands()
        {
            var brands = await _unitOfWork.BrandRepository.GetAll();
             if (brands == null || !brands.Any() ) return NotFound(new ApiResponse(404));

            return Ok(brands);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            var brand = await _unitOfWork.BrandRepository.Get(p => p.Id == id);
            if (brand == null) return NotFound(new ApiResponse(404));

            return Ok(brand);
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> CreateItem(Brand brand)
        {
            _unitOfWork.BrandRepository.Add(brand);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem creating brand"));

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Brand>> UpdateItem(int id, Brand brand)
        {
            if(!IsExists(id))
                return BadRequest("Cannot update this item detail");
                
            
            _unitOfWork.BrandRepository.Update(brand);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating brand"));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var brand = await _unitOfWork.BrandRepository.Get(p => p.Id == id);

            if(brand == null) return  BadRequest(new ApiResponse(400, "Problem deleting brand"));

            _unitOfWork.BrandRepository.Remove(brand);

            var result = await _unitOfWork.Save();

            if(result <= 0) return BadRequest(new ApiResponse(400,"Problem deleting brand"));
            
            return Ok();

        }

        private bool IsExists(int id)
        {
            return _unitOfWork.ItemRepository.IsExists(id);
        }
    }
}
