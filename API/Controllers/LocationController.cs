using API.Dtos.Item;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LocationController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LocationController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Location>>> GetLocations()
        {
            var locations = await _unitOfWork.locationRepository.GetAll();
             if (locations == null || !locations.Any() ) return NotFound(new ApiResponse(404));

            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var location = await _unitOfWork.locationRepository.Get(p => p.Id == id);
            if (location == null) return NotFound(new ApiResponse(404));

            return Ok(location);
        }

        [HttpPost]
        public async Task<ActionResult<Location>> CreateItem(Location location)
        {
            _unitOfWork.locationRepository.Add(location);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem creating Location"));

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Location>> UpdateItem(int id, Location location)
        {
            if(!IsExists(id))
                return BadRequest("Cannot update this item detail");
                
            
            _unitOfWork.locationRepository.Update(location);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating location"));

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var location = await _unitOfWork.locationRepository.Get(p => p.Id == id);

            if(location == null) return  BadRequest(new ApiResponse(400, "Problem deleting location"));

            _unitOfWork.locationRepository.Remove(location);

            var result = await _unitOfWork.Save();

            if(result <= 0) return BadRequest(new ApiResponse(400,"Problem deleting location"));
            
            return Ok();

        }

        private bool IsExists(int id)
        {
            return _unitOfWork.locationRepository.IsExists(id);
        }
    }
}
