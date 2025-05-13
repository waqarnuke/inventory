namespace API.Dtos.Company
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public List<LocationDto>? Locations { get; set; }
        public string? UserId { get; set; }
    }
    public class LocationDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}