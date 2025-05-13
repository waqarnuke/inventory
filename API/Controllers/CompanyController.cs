using API.Dtos.Company;
using API.Errors;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CompanyController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> GetCompanies(string userid)
        {
            var company = await _unitOfWork.companyRepository.Get(x => x.UserId == userid, includeProperties: "Locations");

            if (company == null) return NotFound(new ApiResponse(404));

            var companyDto = new CompanyDto
            {
                Id = company.Id,
                CompanyName = company.Name,
                UserId = company.UserId,
                Locations =  company.Locations != null ?  company.Locations.Select(l => new LocationDto
                {
                    Id = l.Id,
                    Name = l.Name
                }).ToList()
                : null
            };

            return Ok(companyDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CompanyDto>> UpdateItem(CompanyDto company)
        {    
            var existingCompnay = await _unitOfWork.companyRepository.Get(x => x.Id == company.Id);
            
            if(existingCompnay == null) return NotFound(new ApiResponse(404));

            // Update only specific columns
            if(company != null)
            {
                existingCompnay.Name = company.CompanyName ?? "Company";
                existingCompnay.UserId = company.UserId ;
            }
            
            _unitOfWork.companyRepository.Update(existingCompnay);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating location"));

            return Ok(result);
        }
    }
}