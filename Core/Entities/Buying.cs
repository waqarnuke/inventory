using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Buying : BaseEntity
{
    public string? TransactionId { get; set; }
    [Required]
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal? PricePerUnit { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? PaymentMethod { get; set; } // "Cash" or "Card"
    public Guid? UserId { get; set; }

    [ForeignKey("ItemId")]
    public virtual Item? Items { get; set; }

    public int? LocationId { get; set; }
    [ForeignKey("LocationId")]
    public virtual Location? Location { get; set; }
}
