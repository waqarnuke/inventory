using API.Dtos.Item;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ModelController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ModelController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Model>>> GetModels()
        {
            var models = await _unitOfWork.modelRepository.GetAll();
             if (models == null || !models.Any() ) return NotFound(new ApiResponse(404));

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Model>> GetModel(int id)
        {
            var model = await _unitOfWork.modelRepository.Get(p => p.Id == id);
            if (model == null) return NotFound(new ApiResponse(404));

            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult<Model>> CreateItem(Model model)
        {
            _unitOfWork.modelRepository.Add(model);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem creating model"));

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Model>> UpdateItem(int id, Model model)
        {
            if(!IsExists(id))
                return BadRequest("Cannot update this item detail");
                
            
            _unitOfWork.modelRepository.Update(model);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating model"));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var model = await _unitOfWork.modelRepository.Get(p => p.Id == id);

            if(model == null) return  BadRequest(new ApiResponse(400, "Problem deleting model"));

            _unitOfWork.modelRepository.Remove(model);

            var result = await _unitOfWork.Save();

            if(result <= 0) return BadRequest(new ApiResponse(400,"Problem deleting model"));
            
            return Ok();

        }

        private bool IsExists(int id)
        {
            return _unitOfWork.modelRepository.IsExists(id);
        }
    }
}
