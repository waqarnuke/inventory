namespace API.Dtos.UserLocationAssignment;

public class UserLocationAssignmentDto
{
    public int? Id { get; set; }
    public string? UserId { get; set; }
    public int LocationId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? CreatedById { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime DateTime { get; set; }
}
