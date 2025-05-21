using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class UserLocationAssignment : BaseEntity
{

    public string UserId { get; set; } = null!;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int LocationId { get; set; }
    public string? CreatedById { get; set; }
    // Navigation properties (optional)
    public Core.Entities.Location ? Location { get; set; }
}
