using API.Dtos.Item;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class StorageController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StorageController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Storage>>> GetStorages()
        {
            var storages = await _unitOfWork.StorageRepository.GetAll();
             if (storages == null || !storages.Any() ) return NotFound(new ApiResponse(404));

            return Ok(storages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Storage>> GetStorage(int id)
        {
            var storage = await _unitOfWork.StorageRepository.Get(p => p.Id == id);
            if (storage == null) return NotFound(new ApiResponse(404));

            return Ok(storage);
        }

        [HttpPost]
        public async Task<ActionResult<Storage>> CreateItem(Storage storage)
        {
            _unitOfWork.StorageRepository.Add(storage);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem creating Storage"));

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Storage>> UpdateItem(int id, Storage storage)
        {
            if(!IsExists(id))
                return BadRequest("Cannot update this item detail");
                
            
            _unitOfWork.StorageRepository.Update(storage);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating Storage"));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var storage = await _unitOfWork.StorageRepository.Get(p => p.Id == id);

            if(storage == null) return  BadRequest(new ApiResponse(400, "Problem deleting storage"));

            _unitOfWork.StorageRepository.Remove(storage);

            var result = await _unitOfWork.Save();

            if(result <= 0) return BadRequest(new ApiResponse(400,"Problem deleting storage"));
            
            return Ok();

        }

        private bool IsExists(int id)
        {
            return _unitOfWork.ItemRepository.IsExists(id);
        }
    }
}
