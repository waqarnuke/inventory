using System;

namespace API.Dtos.Buying;

public class BuyingRequestDto
{
    public string? PaymentMethod { get; set; }
    public List<BuyingCreateDto>? Items { get; set; }
}
