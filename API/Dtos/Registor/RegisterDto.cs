using System;

namespace API.Dtos.Registor;

public class RegisterDto
{
    public int Id { get; set; }
    public decimal Cash { get; set; }
    public decimal Card { get; set; }
    public DateTime LastUpdated { get; set; }
    public string UserId { get; set; } = null!;
    public int LocationId { get; set; }
    public string? Location { get; set; }
}
