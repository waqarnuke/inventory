using API.Dtos.Dashboard;
using Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DashboardController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        [HttpGet("summary")]
        //[Authorize]
        public async Task<IActionResult> GetDashboardSummary(int locationId)
        {
            var getAllSale = await _unitOfWork.saleRepository.GetAllById(x => x.LocationId ==locationId); 
            var totalSales = getAllSale.Sum(x => x.TotalPrice);

            var getAllBuying = await _unitOfWork.buyingRepository.GetAllById(x => x.LocationId ==locationId); 
            var totalBuying  = getAllBuying.Sum(x => x.TotalPrice);

            var cashBalance = await _unitOfWork.registerRepository.GetAllById(x => x.LocationId ==locationId);  

            // Simulate fetching data from a service or database
            var summary = new DashboardSummaryDto
            {
                TotalSales = totalSales,
                TotalBuying = totalBuying ?? 0,
                RegisterCashBalance = cashBalance.Select(x => x.CashBalance).Sum(),
                RegisterCardBalance = 3000.00m
            };

            return Ok(summary);
        }
    }
}
