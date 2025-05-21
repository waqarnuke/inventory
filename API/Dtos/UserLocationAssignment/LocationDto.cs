using System;

namespace API.Dtos.UserLocationAssignment;

public class LocationAssignmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int CompanyId { get; set; }  // Foreign Key
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? CreatedById { get; set; }

}
