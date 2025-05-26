using API.Dtos.Item;
using API.Dtos.Sale;
using API.Errors;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SaleController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<List<Sale>>> GetItems()
        {
            var sale = await _unitOfWork.saleRepository.GetAll();

            if (sale == null) return NotFound(new ApiResponse(404));

            return Ok(sale);
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] SaleCreateDto newSale)
        {
            if (newSale.Quantity <= 0 || newSale.PricePerUnit <= 0)
                return BadRequest("Invalid quantity or price.");

            // Generate Transaction ID if not provided
            if (string.IsNullOrEmpty(newSale.TransactionId))
                newSale.TransactionId = Guid.NewGuid().ToString();

            // Check if product exists and has enough stock
            var product = await _unitOfWork.itemRepository.Get(x => (x.Title ?? "").ToLower() == (newSale.Title ?? "").ToLower());
            if (product == null || product.Stock < newSale.Quantity)
                return BadRequest($"Not enough stock available for {newSale.Title}.");

            // Calculate Total Price
            newSale.TotalPrice = newSale.Quantity * newSale.PricePerUnit;

            var sale = new Sale
            {
                UserId = newSale.UserId,
                ItemId = product.Id,
                Quantity = newSale.Quantity,
                PricePerUnit = newSale.PricePerUnit,
                TotalPrice = newSale.Quantity * newSale.PricePerUnit,
                TransactionId = newSale.TransactionId,
                PaymentMethod = newSale.PaymentMethod ?? "",
                LocationId = newSale.LocationId
            };
            // Temporarily store item in cart
            _unitOfWork.saleRepository.Add(sale);
            await _unitOfWork.Save();

            return Ok(new { Message = "Item added to cart", TransactionId = newSale.TransactionId });
        }

        [HttpPost("confirm-sale")]
        public async Task<IActionResult> ConfirmSale(string TransactionId,string PaymentMethod)
        {
            var cartItems = await _unitOfWork.saleRepository.GetAllById(s => s.TransactionId == TransactionId);

            if (!cartItems.Any())
                return BadRequest("Cart is empty or transaction ID is invalid.");

            // Calculate total amount
            decimal totalAmount = cartItems.Sum(item => item.TotalPrice);

            // Get register balance
            var getRegister = await _unitOfWork.registerRepository.GetAll() ;
            Register register = getRegister.FirstOrDefault() ?? new Register();
            if (register == null)
            {
                return BadRequest("Register not found.");
            }

            // Deduct stock from inventory & update register
            foreach (var item in cartItems)
            {
                var product = await _unitOfWork.itemRepository.Get(p => p.Id == item.ItemId);
                if (product == null || product.Stock < item.Quantity)
                    return BadRequest($"Stock insufficient for {item.ProductName}.");

                // Reduce stock from inventory
                product.Stock -= item.Quantity;
                _unitOfWork.itemRepository.Update(product);
            }

            // Add amount to register based on payment method
            if (PaymentMethod == "Cash")
            {
                register.CashBalance += totalAmount;
            }
            else if (PaymentMethod == "Card")
            {
                register.CardBalance += totalAmount;
            }
            else
            {
                return BadRequest("Invalid Payment Method.");
            }
            int result = await _unitOfWork.Save();
            // Save updated register balance
            if(result  <= 0) return  BadRequest(new ApiResponse(400, "Problem in creating selling item"));

            return Ok(new { Message = "Sale confirmed successfully", TotalAmount = totalAmount });
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> Confirm(SaleRequestDto itemsSale)
        {
            string getTransId = Guid.NewGuid().ToString();

            if (itemsSale == null || itemsSale.Items == null || !itemsSale.Items.Any())
                return BadRequest("No items to confirm.");

            var totalAmount = itemsSale.Items.Sum(i => i.Quantity * i.PricePerUnit);

            if (itemsSale.LocationId == null || itemsSale.LocationId <= 0)
                return BadRequest("Invalid location ID.");

            var getRegister = await _unitOfWork.saleRegisterRepository.GetAllById(x => x.LocationId == itemsSale.LocationId);
            BaseRegister register = getRegister.FirstOrDefault() ?? new BaseRegister();

            if (register == null)
            {
                return BadRequest("Register not found.");
            }
            
            if (itemsSale.PaymentMethod == "Cash")
            {
                register.Cash += totalAmount;
            }
            else if (itemsSale.PaymentMethod == "Card")
            {
                register.Card += totalAmount;
            }
            else
            {
                return BadRequest("Invalid payment method.");
            }
            // Process each item in the sale
            foreach (var item in itemsSale.Items)
            {
                var product = await _unitOfWork.itemRepository.Get(p => p.Id == item.ItemId);
                if (product == null || product.Stock < item.Quantity)
                    return BadRequest($"Stock insufficient for {item.Title}.");

                // Reduce stock from inventory
                product.Stock -= item.Quantity;
                // Create a new Sale entity
                var sale = new Sale
                {
                    TransactionId = getTransId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    PricePerUnit = item.PricePerUnit,
                    TotalPrice = item.Quantity * item.PricePerUnit,
                    PaymentMethod = itemsSale.PaymentMethod,
                    LocationId = item.LocationId,
                };
                _unitOfWork.saleRepository.Add(sale);
            }

            // Save changes to the database
            int result = await _unitOfWork.Save();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem in creating selling item"));

            return Ok(new { Message = "Sale confirmed successfully" });
        }
        
        [HttpGet("get-sale-by-transaction")]
        public async Task<ActionResult<PagedResultDto<SaleTransactionDto>>> GetSaleByTransaction(int LocationId,int index, int size, string? orderBy = null, bool ascending = true,string? search = null)
        {
            var today = DateTime.UtcNow.Date;
            var sale = await _unitOfWork.saleRepository.GetAllById(x=>x.LocationId == LocationId, includeProperties: "Items,Location");

            if (sale == null || !sale.Any())
                return NotFound(new ApiResponse(404, "Sale not found."));

            var groupSales = sale.GroupBy(s => s.TransactionId)
                .Select(g => new SaleTransactionDto
                {
                    TransactionId = g.Key,
                    SaleDate = g.First().CreatedTime,
                    PaymentMethod = g.First().PaymentMethod,
                    Quantity = g.Sum(s => s.Quantity),
                    PricePerUnit = g.First().PricePerUnit,
                    TotalPrice = g.Sum(s => s.TotalPrice),
                    Items = g.Select(s => new ItemToReturnDto
                    {
                        Id = s.ItemId,
                        Title = s.Items != null ? s.Items.Title : string.Empty,
                        Stock = s.Quantity,
                        Price = s.Items != null ? s.Items.Price : 0,
                    }).ToList()
                }).OrderByDescending(s => s.SaleDate).ToList();

            var result = new PagedResultDto<SaleTransactionDto>
            {
                Data = groupSales,
                TotalCount = groupSales.Count,
                PageSize = 10, // Set your desired page size
                PageIndex = 1, // Set your desired page index
            };
            
            return Ok(result);
        }
    }
}
