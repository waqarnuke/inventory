using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class BaseRegister
{
    [Key]
    public int Id { get; set; }
    public decimal? Cash { get; set; }
    public decimal? Card { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? UserId { get; set; }

    //[ForeignKey("LocationId")]
    public int LocationId { get; set; }              // FK
    public Location? Location { get; set; } // Navigation
}
