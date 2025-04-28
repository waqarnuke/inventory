using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Sale : BaseEntity
{
    public string? TransactionId { get; set; } // Unique Transaction ID for bulk sales
    public int ItemId { get; set; }
    public string ProductName { get; set; }  = string.Empty;
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal TotalPrice { get; set; }
    public string PaymentMethod { get; set; }  = string.Empty;
    public Guid? UserId { get; set; }
    [ForeignKey("ItemId")]
    public virtual Item? Items { get; set; }
    public int? LocationId { get; set; }
    [ForeignKey("LocationId")]
    public virtual Location? Location { get; set; }
}
