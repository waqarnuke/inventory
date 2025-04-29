using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Company :BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public ICollection<Location>? Locations { get; set; } = new List<Location>();
}
