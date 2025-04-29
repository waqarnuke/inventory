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
                Locations =  company.Locations != null ?  company.Locations.Select(l => new LocationDto
                {
                    Id = l.Id,
                    LocationName = l.Name
                }).ToList()
                : null
            };

            return Ok(companyDto);
        }
    }
}