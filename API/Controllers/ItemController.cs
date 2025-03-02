using API.Dtos.Item;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ItemController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ItemController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ItemToReturnDto>>> GetItems()
        {
            var items = await _unitOfWork.ItemRepository.GetAll(includeProperties: "Brand,Model,Location,MobileNetwork,Storage");
            var itemsToReturn = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemToReturnDto>>(items);
            if (itemsToReturn == null) return NotFound(new ApiResponse(404));

            return Ok(itemsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemToReturnDto>> GetItem(int id)
        {
            var item  = await _unitOfWork.ItemRepository.Get(p => p.Id == id,includeProperties:"Brand,Model,Location,MobileNetwork,Storage,ItemType");

            var itemToReturn = _mapper.Map<Item,ItemToReturnDto>(item);

            if (itemToReturn == null) return NotFound(new ApiResponse(404));

            return  Ok(itemToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<ItemCreateDto>> CreateItem(ItemCreateDto itemCreateDto)
        {
            if( (Common.ItemType)itemCreateDto.ItemTypeId == Common.ItemType.Anonymous){
                
                itemCreateDto.EMI = null;
                itemCreateDto.Stock = 1;

            }
            else if((Common.ItemType)itemCreateDto.ItemTypeId == Common.ItemType.Single){
                
                var emiItem = await _unitOfWork.ItemRepository.Get(p => p.EMI == itemCreateDto.EMI);
                
                if(emiItem != null) return BadRequest(new ApiResponse(400, "Item already exists"));

                itemCreateDto.Stock = 1;

            }
            else
            {
                itemCreateDto.EMI = null;
            }
            
            Item item =new(){
                Title = itemCreateDto.Title,
                Description = itemCreateDto.Description,
                BrandId = itemCreateDto.BrandId == 0 ? null : itemCreateDto.BrandId,
                ModelId = itemCreateDto.ModelId == 0 ? null : itemCreateDto.ModelId,
                Price = itemCreateDto.Price,
                Stock = itemCreateDto.Stock,
                EMI = itemCreateDto.EMI,
                IsSingle = itemCreateDto.IsSingle,
                ImageUrl = itemCreateDto.ImageUrl,
                Color = itemCreateDto.Color,
                Condition = itemCreateDto.Condition,
                ItemTypeId = itemCreateDto.ItemTypeId == 0 ? null : itemCreateDto.ItemTypeId,
                LocationId = itemCreateDto.LocationId == 0 ? null : itemCreateDto.LocationId,
                MobileNetworkId = itemCreateDto.MobileNetworkId == 0 ? null : itemCreateDto.MobileNetworkId,
                StorageId = itemCreateDto.StorageId == 0 ? null : itemCreateDto.StorageId
            };
            //product.ImageUrl = "images/products/placeholder.png";
            _unitOfWork.ItemRepository.Add(item);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem creating product"));

            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ItemCreateDto>> UpdateItem(int id, ItemCreateDto itemCreateDto)
        {
            if(!IsExists(id))
                return BadRequest("Cannot update this item detail");
                
            //var itme = await _unitOfWork.ItemRepository.Get(p => p.Id == id);
            
            if( (Common.ItemType)itemCreateDto.ItemTypeId == Common.ItemType.Anonymous){
                
                itemCreateDto.EMI = null;
                itemCreateDto.Stock = 1;

            }
            else if((Common.ItemType)itemCreateDto.ItemTypeId == Common.ItemType.Single)
            {
                var isDuplicateEmi = await _unitOfWork.ItemRepository.Get(p => p.EMI == itemCreateDto.EMI && p.Id != id);
                
                if(isDuplicateEmi != null) return BadRequest(new ApiResponse(400, "Item already exists"));
                
                itemCreateDto.Stock = 1;

            }
            else
            {
                itemCreateDto.EMI = null;
            }

            Item item =new(){
                Title = itemCreateDto.Title,
                Description = itemCreateDto.Description,
                BrandId = itemCreateDto.BrandId == 0 ? null : itemCreateDto.BrandId,
                ModelId = itemCreateDto.ModelId == 0 ? null : itemCreateDto.ModelId,
                Price = itemCreateDto.Price,
                Stock = itemCreateDto.Stock,
                EMI = itemCreateDto.EMI,
                IsSingle = itemCreateDto.IsSingle,
                ImageUrl = itemCreateDto.ImageUrl,
                Id = id,
                Color = itemCreateDto.Color,
                Condition = itemCreateDto.Condition,
                ItemTypeId = itemCreateDto.ItemTypeId == 0 ? null : itemCreateDto.ItemTypeId,
                LocationId = itemCreateDto.LocationId == 0 ? null : itemCreateDto.LocationId,
                MobileNetworkId = itemCreateDto.MobileNetworkId == 0 ? null : itemCreateDto.MobileNetworkId,
                StorageId = itemCreateDto.StorageId == 0 ? null : itemCreateDto.StorageId
            };
            _unitOfWork.ItemRepository.Update(item);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating product"));

            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var item = await _unitOfWork.ItemRepository.Get(p => p.Id == id);

            _unitOfWork.ItemRepository.Remove(item);

            var result = await _unitOfWork.Save();

            if(result <= 0) return BadRequest(new ApiResponse(400,"Problem deleting product"));
            
            return Ok();

        }

        private bool IsExists(int id)
        {
            return _unitOfWork.ItemRepository.IsExists(id);
        }
    }
}
