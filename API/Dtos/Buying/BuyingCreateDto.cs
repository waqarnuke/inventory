using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Buying;

public class BuyingCreateDto
{
    public int ItemId { get; set; }
    public string? Title { get; set; }
    public int Quantity { get; set; }
    public decimal? PricePerUnit { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? PaymentMethod { get; set; } // "Cash" or "Card"
    public Guid? UserId { get; set; }
    public string? TransactionId { get; set; }
    public int? LocationId { get; set; }
}
