using API.Dtos.Registor;
using API.Errors;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuyRegisterController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BuyRegisterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<List<BuyRegister>>> GetBuyRegisters()
        {
            var buyRegisters = await _unitOfWork.buyRegisterRepository.GetAll();
            return Ok(buyRegisters);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<RegisterDto>> GetBuyRegister(string id)
        {
            var buyRegister = await _unitOfWork.buyRegisterRepository.GetAllById(x => x.UserId == id,includeProperties: "Location");
            if (buyRegister == null) return NotFound();

            var buyRegisterDto = buyRegister.Select( res => new RegisterDto
            {
                Id = res.Id,
                Cash = res.Cash ?? 0,
                Card = res.Card ?? 0,
                LastUpdated = res.LastUpdated ?? DateTime.Now,
                UserId = res.UserId ?? string.Empty,
                LocationId = res.LocationId,
                Location = res.Location?.Name
            }).ToList();

            return Ok(buyRegisterDto);
        }

        [HttpPost]
        public async Task<ActionResult<BuyRegister>> CreateBuyRegister(RegisterDto buyRegisterDto)
        {
            if (buyRegisterDto == null) return BadRequest("Buy register is null");
            
            var existingLocation = await _unitOfWork.buyRegisterRepository.Get(x => x.LocationId == buyRegisterDto.LocationId);
            if (existingLocation != null)
            {
                return BadRequest(new ApiResponse(400, "Buy register already exists for this user and location"));
            }
            
            var buyRegister = new BuyRegister
            {
                Cash = buyRegisterDto.Cash,
                Card = buyRegisterDto.Card,
                LastUpdated = buyRegisterDto.LastUpdated,
                UserId = buyRegisterDto.UserId,
                LocationId = buyRegisterDto.LocationId
            };
            _unitOfWork.buyRegisterRepository.Add(buyRegister);

            var result = await _unitOfWork.Save();

            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem creating register"));

            return Ok(buyRegister.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBuyRegister(int id, RegisterDto buyRegisterDto)
        {
            if (buyRegisterDto == null) return BadRequest("Buy register is null");

            var existingRegister = await _unitOfWork.buyRegisterRepository.Get(x => x.Id == id);

            if (existingRegister == null) return NotFound();

            existingRegister.Cash = buyRegisterDto.Cash;
            existingRegister.LastUpdated = DateTime.Now;
            existingRegister.LocationId = buyRegisterDto.LocationId;

            var result = await _unitOfWork.Save();

            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating register"));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuyRegister(int id)
        {
            var buyRegister = await _unitOfWork.buyRegisterRepository.Get(x => x.Id == id);

            if (buyRegister == null) return NotFound();

            _unitOfWork.buyRegisterRepository.Remove(buyRegister);

            var result = await _unitOfWork.Save();

            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating register"));

            return NoContent();
        }
    }
}
