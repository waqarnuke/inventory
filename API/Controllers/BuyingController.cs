using API.Dtos.Buying;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuyingController :BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BuyingController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<List<Buying>>> GetItems()
        {
            var buying = await _unitOfWork.buyingRepository.GetAll();
            //var itemsToReturn = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemToReturnDto>>(items);
            if (buying == null) return NotFound(new ApiResponse(404));

            return Ok(buying);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Buying>> GetById(int id)
        {
            var buying = await _unitOfWork.buyingRepository.Get(x => x.Id == id);
            //var itemsToReturn = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemToReturnDto>>(items);
            if (buying == null) return NotFound(new ApiResponse(404));

            return Ok(buying);
        }

        [HttpPost("add-to-cart")]
        public async Task<ActionResult> AddToCart(BuyingCreateDto newPurchase)
        {
            if (newPurchase.Quantity <= 0 || newPurchase.PricePerUnit <= 0)
                return BadRequest("Invalid quantity or price.");

             // Generate unique transaction ID if not provided
            if (string.IsNullOrEmpty(newPurchase.TransactionId))
                newPurchase.TransactionId = Guid.NewGuid().ToString();   

            var buying = new Buying
            {
                ItemId = newPurchase.ItemId,
                Quantity = newPurchase.Quantity,
                PricePerUnit = newPurchase.PricePerUnit,
                TotalPrice = newPurchase.Quantity * newPurchase.PricePerUnit,
                TransactionId = newPurchase.TransactionId,
                PaymentMethod = newPurchase.PaymentMethod
            };
            // Save item to Buying Table (Temporary)
            _unitOfWork.buyingRepository.Add(buying);
            int result = await _unitOfWork.Save();

            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem creating Buying item"));

            return Ok(new { Message = "Item added", TransactionId = newPurchase.TransactionId });
        }

        [HttpPost("confirm-purchase")]
        public async Task<ActionResult> ConfirmPurchase(string TransactionId,string PaymentMethod)
        {
            var cartItems = await _unitOfWork.buyingRepository.GetAllById(x => x.TransactionId == TransactionId);

            if (!cartItems.Any())
                return BadRequest("Cart is empty or transaction ID is invalid.");

            // Calculate total amount
            decimal totalAmount = cartItems.Sum(item => item.TotalPrice.Value);  

            // Get register balance
            var getRegister = await _unitOfWork.registerRepository.GetAll() ;
            Register register = getRegister.FirstOrDefault();

            if (register == null)
            {
                return BadRequest("Register not found.");
            }
            // Deduct amount based on payment method
            if (PaymentMethod == "Cash")
            {
                if (register.CashBalance < totalAmount)
                    return BadRequest("Insufficient Cash Balance.");
                register.CashBalance -= totalAmount;
            }
            else if (PaymentMethod == "Card")
            {
                if (register.CardBalance < totalAmount)
                    return BadRequest("Insufficient Card Balance.");
                register.CardBalance -= totalAmount;
            }
            else
            {
                return BadRequest("Invalid Payment Method.");
            }
            
            // Update stock in product table
            foreach (var item in cartItems)
            {
                var eachItem = await _unitOfWork.itemRepository.Get(x => x.Id == item.ItemId);
                if (eachItem != null)
                {
                    ///If product exists, update its quantity
                    eachItem.Stock += item.Quantity;
                    _unitOfWork.itemRepository.Update(eachItem);
                }
                else
                {
                    return BadRequest("No product found"); 
                }
            }

            int result = await _unitOfWork.Save();

            if(result  <= 0) return  BadRequest(new ApiResponse(400, "Problem in creating purchase item"));
            
            return Ok(new { Message = "Purchase confirmed successfully", TotalAmount = totalAmount });
        }
        
        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> CancelCart(string transactionId)
        {
            var cartItems = await _unitOfWork.buyingRepository.GetAllById(x => x.TransactionId == transactionId);

            if(!cartItems.Any())
            {
                return NotFound("Cart is empty or invalid transaction ID.");
            }
                
            _unitOfWork.buyingRepository.RemoveRange(cartItems);

            int result = await _unitOfWork.Save();

            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem Deleting item"));

            return Ok(new { Message = "Cart cleared successfully" });
        }

        
        // [HttpPost]
        // public async Task<ActionResult<Buying>> CreateItem1(BuyingCreateDto newPurchase)
        // {
        //     if (newPurchase.Quantity <= 0 || newPurchase.PricePerUnit <= 0)
        //         return BadRequest("Invalid quantity or price.");

        //     var item = await _unitOfWork.itemRepository.Get(x => x.Title.ToLower() == newPurchase.Title.ToLower());
            
        //     if (item != null)
        //     {   
        //         item.Stock += newPurchase.Quantity;
        //         _unitOfWork.itemRepository.Update(item);
        //         await _unitOfWork.Save();
        //     }

        //     var buying = new Buying
        //     {
        //         ItemId = item.Id,
        //         Quantity = newPurchase.Quantity,
        //         PricePerUnit = newPurchase.PricePerUnit,
        //         TotalPrice = newPurchase.Quantity * newPurchase.PricePerUnit,
        //         PaymentMethod = newPurchase.PaymentMethod
        //     };

        //     _unitOfWork.buyingRepository.Add(buying);
        //     int result = await _unitOfWork.Save();

        //     if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem creating Buying item"));
            
        //     return Ok("Product added.");
        // }
    }
}
