using System;

namespace API.Dtos.Dashboard;

public class DashboardSummaryDto
{
    public decimal TotalSales { get; set; }
    public decimal TotalBuying { get; set; }
    public decimal RegisterCashBalance { get; set; }
    public decimal RegisterCardBalance { get; set; }
}
