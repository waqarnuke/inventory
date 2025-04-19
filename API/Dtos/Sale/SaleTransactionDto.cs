using System;
using API.Dtos.Item;

namespace API.Dtos.Sale;

public class SaleTransactionDto
{
    public int ItemId { get; set; }
    public string Title { get; set; }
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal TotalPrice { get; set; }
    public string? PaymentMethod { get; set; }
    public Guid? UserId { get; set; }
    public string? TransactionId { get; set; }
    public int? LocationId { get; set; }
    public DateTime? SaleDate { get; set; } 
    public IReadOnlyList<ItemToReturnDto>? Items { get; set; }
}
