using API.Dtos.Item;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MobileNetworkController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MobileNetworkController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<MobileNetwork>>> GetMobileNetworks()
        {
            var mobileNetworks = await _unitOfWork.mobileNetworkRepository.GetAll();
             if (mobileNetworks == null || !mobileNetworks.Any() ) return NotFound(new ApiResponse(404));

            return Ok(mobileNetworks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MobileNetwork>> GetMobileNetwork(int id)
        {
            var mobileNetwork = await _unitOfWork.mobileNetworkRepository.Get(p => p.Id == id);
            if (mobileNetwork == null) return NotFound(new ApiResponse(404));

            return Ok(mobileNetwork);
        }

        [HttpPost]
        public async Task<ActionResult<MobileNetwork>> CreateItem(MobileNetwork mobileNetwork)
        {
            _unitOfWork.mobileNetworkRepository.Add(mobileNetwork);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem creating MobileNetwork"));

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MobileNetwork>> UpdateItem(int id, MobileNetwork mobileNetwork)
        {
            if(!IsExists(id))
                return BadRequest("Cannot update this item detail");
                
            
            _unitOfWork.mobileNetworkRepository.Update(mobileNetwork);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating MobileNetwork"));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var mobileNetwork = await _unitOfWork.mobileNetworkRepository.Get(p => p.Id == id);

            if(mobileNetwork == null) return  BadRequest(new ApiResponse(400, "Problem deleting MobileNetwork"));

            _unitOfWork.mobileNetworkRepository.Remove(mobileNetwork);

            var result = await _unitOfWork.Save();

            if(result <= 0) return BadRequest(new ApiResponse(400,"Problem deleting MobileNetwork"));
            
            return Ok();

        }

        private bool IsExists(int id)
        {
            return _unitOfWork.ItemRepository.IsExists(id);
        }
    }
}
