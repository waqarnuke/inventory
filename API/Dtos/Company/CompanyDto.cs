using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos.Company
{
    public class CompanyDto
    {
        public int Id { get; set; }
    public string? CompanyName { get; set; }
    public List<LocationDto>? Locations { get; set; }
    }
    public class LocationDto
{
    public int Id { get; set; }
    public string? LocationName { get; set; }
}
}