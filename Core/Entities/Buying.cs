using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Buying : BaseEntity
{
    [Required]
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal? PricePerUnit { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? PaymentMethod { get; set; } // "Cash" or "Card"
    public Guid? UserId { get; set; }
}
