using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SupplierController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupplierController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Supplier>>> GetBrands()
        {
            var suppliers = await _unitOfWork.supplierRepository.GetAll();
            if (suppliers == null || !suppliers.Any() ) return NotFound(new ApiResponse(404));
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetBrand(int id)
        {
            var supplier = await _unitOfWork.supplierRepository.Get(p => p.Id == id);
            if (supplier == null) return NotFound(new ApiResponse(404));

            return Ok(supplier);
        }
        [HttpPost]
        public async Task<ActionResult<Supplier>> CreateItem(Supplier supplier)
        {
            _unitOfWork.supplierRepository.Add(supplier);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem creating supplier"));

            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Supplier>> UpdateItem(int id, Supplier supplier)
        {
            if(!IsExists(id))
                return BadRequest("Cannot update this item detail");
                
            
            _unitOfWork.supplierRepository.Update(supplier);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating brand"));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var supplier = await _unitOfWork.supplierRepository.Get(p => p.Id == id);

            if(supplier == null) return  BadRequest(new ApiResponse(400, "Problem deleting brand"));

            _unitOfWork.supplierRepository.Remove(supplier);

            var result = await _unitOfWork.Save();

            if(result <= 0) return BadRequest(new ApiResponse(400,"Problem deleting brand"));
            
            return Ok();

        }

        private bool IsExists(int id)
        {
            return _unitOfWork.supplierRepository.IsExists(id);
        }
    }
}
