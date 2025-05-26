using API.Dtos.Registor;
using API.Errors;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SaleRegisterController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaleRegisterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<List<SaleRegister>>> GetSellRegisters()
        {
            var sellRegisters = await _unitOfWork.saleRegisterRepository.GetAll();
            return Ok(sellRegisters);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<RegisterDto>> GetSellRegister(string id)
        {
            var sellRegister = await _unitOfWork.saleRegisterRepository.GetAllById(x => x.UserId == id,includeProperties: "Location");
            if (sellRegister == null) return NotFound();

            var sellRegisterDto = sellRegister.Select( res => new RegisterDto
            {
                Id = res.Id,
                Cash = res.Cash ?? 0,
                Card = res.Card ?? 0,
                LastUpdated = res.LastUpdated ?? DateTime.Now,
                UserId = res.UserId ?? string.Empty,
                LocationId = res.LocationId,
                Location = res.Location?.Name
            }).ToList();


            return Ok(sellRegisterDto);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateSellRegister(RegisterDto sellRegisterDto)
        {
            if (sellRegisterDto == null) return BadRequest("Sell register is null");

            var existingLocation = await _unitOfWork.saleRegisterRepository.Get(x => x.LocationId == sellRegisterDto.LocationId && x.UserId == sellRegisterDto.UserId);

            if (existingLocation != null)
            {
                return BadRequest(new ApiResponse(400, "Sell register already exists for this user and location"));
            }
        
            var sellRegister = new SaleRegister
            {
                Cash = sellRegisterDto.Cash,
                Card = sellRegisterDto.Card,
                LastUpdated = sellRegisterDto.LastUpdated,
                UserId = sellRegisterDto.UserId,
                LocationId = sellRegisterDto.LocationId
            };

            _unitOfWork.saleRegisterRepository.Add(sellRegister);

            var result = await _unitOfWork.Save();

            if(result <= 0) return  BadRequest(new ApiResponse    (400, "Problem creating register"));

            return Ok(sellRegister.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSellRegister(int id, SaleRegister saleRegister)
        {
            if (saleRegister == null) return BadRequest("Buy register is null");

            var existingRegister = await _unitOfWork.saleRegisterRepository.Get(x => x.Id == id);

            if (existingRegister == null) return NotFound();

            existingRegister.Cash = saleRegister.Cash;
            existingRegister.LastUpdated = DateTime.Now;
            existingRegister.LocationId = saleRegister.LocationId;;

            var result = await _unitOfWork.Save();

            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating register"));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSellRegister(int id)
        {
            var sellRegister = await _unitOfWork.saleRegisterRepository.Get(x => x.Id == id);
            if (sellRegister == null) return NotFound();

            _unitOfWork.saleRegisterRepository.Remove(sellRegister);

            var result = await _unitOfWork.Save();

            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem deleting register"));

            return NoContent();
        }
    }
}
