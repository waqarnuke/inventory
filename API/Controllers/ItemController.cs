using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Dtos;
using API.Dtos.Buying;
using API.Dtos.Item;
using API.Dtos.Sale;
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
        private readonly IImageService _imageService;
        private readonly IImageRepository photorepo;
        public ItemController(IUnitOfWork unitOfWork,IMapper mapper,IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;

        }

        [HttpGet]
        public async Task<ActionResult<List<ItemToReturnDto>>> GetItems()
        {
            var items = await _unitOfWork.itemRepository.GetAll(includeProperties: "Brand,Model,Location,MobileNetwork,Storage,Images");
            var itemsToReturn = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemToReturnDto>>(items);
            //var itemsToReturn = _mapper.Map<IList<Item>, IList<ItemToReturnDto>>(items);
            // var itemsToReturn = _mapper.Map<IEnumerable<Image>, IEnumerable<ImageDto>>;
            if (itemsToReturn == null) return NotFound(new ApiResponse(404));

            return Ok(itemsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemToReturnDto>> GetItem(int id)
        {
            var item  = await _unitOfWork.itemRepository.Get(p => p.Id == id,includeProperties:"Brand,Model,Location,MobileNetwork,Storage,ItemType,Images");

            var itemToReturn = _mapper.Map<Item,ItemToReturnDto>(item);
            //var imageToReturn = _mapper.Map<ICollection<Image>,ICollection<ImageDto>>(item.Images);

            if (itemToReturn == null) return NotFound(new ApiResponse(404));

            return  Ok(itemToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<ItemCreateDto>> CreateItem(ItemCreateDto itemCreateDto)
        {
            // later we check duplicate name
            var IsExists = _unitOfWork.itemRepository.GetAllById(x => x.Title == itemCreateDto.Title);

            if( (Common.ItemType)itemCreateDto.ItemTypeId == Common.ItemType.Anonymous){
                
                itemCreateDto.EMI = null;
            }
            else if((Common.ItemType)itemCreateDto.ItemTypeId == Common.ItemType.Single){
                
                var emiItem = await _unitOfWork.itemRepository.Get(p => p.EMI == itemCreateDto.EMI);
                
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
                StorageId = itemCreateDto.StorageId == 0 ? null : itemCreateDto.StorageId,
                SupplierId = itemCreateDto.SupplierId == 0 ? null : Convert.ToInt32(itemCreateDto.SupplierId) 
            };
            //product.ImageUrl = "images/products/placeholder.png";
            _unitOfWork.itemRepository.Add(item);
            
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

            }
            else if((Common.ItemType)itemCreateDto.ItemTypeId == Common.ItemType.Single)
            {
                var isDuplicateEmi = await _unitOfWork.itemRepository.Get(p => p.EMI == itemCreateDto.EMI && p.Id != id);
                
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
                StorageId = itemCreateDto.StorageId == 0 ? null : itemCreateDto.StorageId,
                SupplierId = itemCreateDto.SupplierId == 0 ? null : itemCreateDto.SupplierId
            };
            _unitOfWork.itemRepository.Update(item);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating product"));

            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            //check photo 
            var photos = await _unitOfWork.imageRepository.GetAllById(x => x.ItemId == id);

            if(photos.Count > 0)
            {
                var photoIds = photos.Select(p => p.PublicId).ToList();

                var delResult = await _imageService.DeleteRangePhoto(photoIds);

                if (delResult.Error != null) return BadRequest(delResult.Error.Message);

                _unitOfWork.imageRepository.RemoveRange(photos);

                var phoneResult = await _unitOfWork.Save();
            }

            var item = await _unitOfWork.itemRepository.Get(p => p.Id == id);

            _unitOfWork.itemRepository.Remove(item);

            var result = await _unitOfWork.Save();

            if(result <= 0) return BadRequest(new ApiResponse(400,"Problem deleting product"));
            
            return Ok();

        }

        [HttpPost("upload-images")]
        public async Task<ActionResult<IReadOnlyList<PhotoDto>>> AddPhoto([FromForm] int itemId, [FromForm] List<IFormFile> images) //IFormFile file,int id
        {
            var imageResult = images.FirstOrDefault();
            if(imageResult == null) return BadRequest("No image found");

            var result = await _imageService.AddPhoto(imageResult);
            
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Image
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                ItemId = itemId
            };

            _unitOfWork.imageRepository.Add(photo);

            int resultSave = await _unitOfWork.Save();

            if(resultSave > 0)
            {
                var getAllphoto = await _unitOfWork.imageRepository.GetAllById(x => x.ItemId == itemId);
                var response = getAllphoto.Select(x => new PhotoDto {
                    Id = x.Id, 
                    PublicId = x.PublicId,
                    Url = x.Url,
                    UserId = x.UserId,
                    ItemId = x.ItemId.Value
                } ).ToList();

                return response;
            }

            return BadRequest();
        }

        [HttpDelete("deletephoto/{photoId:int}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var photo = await _unitOfWork.imageRepository.Get(x => x.Id == photoId);

            if (photo == null) return BadRequest("This photo cannot be deleted");

            if (photo.PublicId != null)
            {
                var result = await _imageService.DeletePhoto(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            _unitOfWork.imageRepository.Remove(photo);

            if(await _unitOfWork.Save() > 0)
            {
                return NoContent();
            }

            return BadRequest("Problem deleting photo");
        }
        private bool IsExists(int id)
        {
            return _unitOfWork.itemRepository.IsExists(id);
        }

        [HttpPost("buy/{id}/{quantity}")]
        public async Task<IActionResult> BuyProduct(int id, int quantity)
        {
            Item item = await _unitOfWork.itemRepository.Get(x => x.Id == id);
            if (item == null)
                return NotFound("Product not found.");
            
            if (item.Stock < quantity)
                return BadRequest("Not enough stock available.");
            
            item.Stock -= quantity;
            _unitOfWork.itemRepository.Add(item);
            await _unitOfWork.Save();
            return Ok("Purchase successful. Stock updated.");
        }
    
        [HttpGet("getPaginatedItems")]
        public async Task<ActionResult<PagedResultDto<ItemToReturnDto>>> GetPaginatedItems(int index, int size, string orderBy = null, bool ascending = true,string search = null)
        {
            // ✅ Optional filter setup
            Expression<Func<Item, bool>> filter = null;
            if (!string.IsNullOrWhiteSpace(search))
            {
                filter = x => x.Title.Contains(search);
            }
           //filter: x => x.Title.Contains(filter) || x.Description.Contains(filter) || x.Brand.Name.Contains(filter) || x.Model.Name.Contains(filter) || x.Location.Name.Contains(filter) || x.MobileNetwork.Name.Contains(filter) || x.Storage.Name.Contains(filter)
            var items = await _unitOfWork.itemRepository.GetPagination(index, size, orderBy, ascending, 
                                                                        includeProperties: "Brand,Model,Location,MobileNetwork,Storage",
                                                                        filter: filter);
            var itemsToReturn = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemToReturnDto>>(items.Items);
            
            if (itemsToReturn == null) return NotFound(new ApiResponse(404));

            var result = new PagedResultDto<ItemToReturnDto>
            {
                Data = itemsToReturn,
                TotalCount = items.TotalCount,
                PageSize = items.PageSize,
                PageIndex = items.PageIndex,
            };

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string query)
        {
            var items  = await _unitOfWork.itemRepository.GetAllById(p => p.Title.Contains(query) || p.EMI == query,includeProperties:"Brand,Model,Location,MobileNetwork,Storage,ItemType,Images");

            var itemToReturn = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemToReturnDto>>(items);;
            
            var result =itemToReturn.Select(res => new SaleCreateDto{
                ItemId = res.Id,
                Title = res.Title,
                Quantity = res.Stock,
                PricePerUnit = Convert.ToDecimal(res.Price),
                LocationId = res.LocationId
            }).ToList();

            if (result == null) return NotFound(new ApiResponse(404));

            return  Ok(result);
        }

        [HttpPost("createByingItem")]
        public async Task<ActionResult<BuyingCreateDto>> CreateByingItem(ItemCreateDto itemCreateDto)
        {
            // later we check duplicate name
            var IsExists = _unitOfWork.itemRepository.GetAllById(x => x.Title == itemCreateDto.Title);

            if( (Common.ItemType)itemCreateDto.ItemTypeId == Common.ItemType.Anonymous){
                
                itemCreateDto.EMI = null;
                //itemCreateDto.Stock = 1;

            }
            else if((Common.ItemType)itemCreateDto.ItemTypeId == Common.ItemType.Single){
                
                var emiItem = await _unitOfWork.itemRepository.Get(p => p.EMI == itemCreateDto.EMI);
                
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
                StorageId = itemCreateDto.StorageId == 0 ? null : itemCreateDto.StorageId,
                SupplierId = itemCreateDto.SupplierId == 0 ? null : Convert.ToInt32(itemCreateDto.SupplierId) 
            };
            //product.ImageUrl = "images/products/placeholder.png";
            _unitOfWork.itemRepository.Add(item);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem creating product"));

            BuyingCreateDto buyingItem = new(){
                ItemId = item.Id,
                Title = item.Title,
                Quantity = item.Stock,
                PricePerUnit = Convert.ToDecimal(item.Price),
                LocationId = item.LocationId
            };

            return Ok(buyingItem);
        }
    
    }
}
