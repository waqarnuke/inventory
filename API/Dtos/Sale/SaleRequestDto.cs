using System;

namespace API.Dtos.Sale;

public class SaleRequestDto
{
    public string PaymentMethod { get; set; }
    public List<SaleCreateDto> Items { get; set; }
}
