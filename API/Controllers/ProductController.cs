using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IWebHostEnvironment _evn;

        public ProductController(IUnitOfWork unitOfWork,IMapper mapper, IPhotoService photoService,IWebHostEnvironment env)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _evn = env;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductToReturnDto>>> GetProducts()
        {
            var product  = await _unitOfWork.productRepository.GetAll(includeProperties:"Category,Photos");

            //return Ok(product);
            return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(product)); 
        } 

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product  = await _unitOfWork.productRepository.Get(p => p.Id == id,includeProperties:"Category,Photos");

            return _mapper.Map<Product,ProductToReturnDto>(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(ProductCreateDto productToCreateDto)
        {
            var product = _mapper.Map<ProductCreateDto,Product>(productToCreateDto);
            //product.ImageUrl = "images/products/placeholder.png";
            _unitOfWork.productRepository.Add(product);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem creating product"));

            return Ok(product);
        } 

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, ProductCreateDto productToUpdate)
        {
            var product = await _unitOfWork.productRepository.Get(p => p.Id == id);
            
            _mapper.Map(productToUpdate,product);

            _unitOfWork.productRepository.Update(product);
            
            var result = await _unitOfWork.Save();
            
            if(result <= 0) return  BadRequest(new ApiResponse(400, "Problem updating product"));

            return Ok(product);
        } 

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.productRepository.Get(p => p.Id == id);

            _unitOfWork.productRepository.Remove(product);

            var result = await _unitOfWork.Save();

            if(result <= 0) return BadRequest(new ApiResponse(400,"Problem deleting product"));
            
            return Ok();

        }
        [HttpPut("{id}/photo")]
        public async Task<ActionResult<ProductToReturnDto>> AddProductPhoto(int id, [FromForm] ProductPhotoDto photoDto)
        {
            var photo = new Photo();
            var product = await _unitOfWork.productRepository.Get(p => p.Id == id);
            if(photoDto.file.Length > 0)
            {
                //var photo = await _photoService.SaveToDiskAsync(photoDto.file);
                //start
                var fileName = Guid.NewGuid() + Path.GetExtension(photoDto.file.FileName);
                        var filePath = Path.Combine("Content/images/products", fileName);
                        await using var fileStream = new FileStream(filePath, FileMode.Create);
                        await photoDto.file.CopyToAsync(fileStream);
                        
                        photo.FileName = fileName;
                        photo.PictureUrl = "images/products/" + fileName;  
                    
                //ed=nd
                if (photo != null)
                {
                    product.AddPhoto(photo.PictureUrl, photo.FileName);
                    _unitOfWork.productRepository.Update(product);
                    var result = await _unitOfWork.Save();

                    if (result <= 0) return BadRequest(new ApiResponse(400, "Problem adding photo product"));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "problem saving photo to disk"));
                }
                
            }

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
    
        [Route("SaveFile")]
        [HttpPost]
        public async Task<ActionResult> SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                if(postedFile != null)
                {
                    string fileName = postedFile.FileName;
                   
                    var oldfile = await _unitOfWork.productRepository.Get(p => p.ImageUrl ==  fileName);
                    string val = _evn.ContentRootPath + "/Content/images/products/";
                    if(oldfile != null)
                    {
                        string oldImagePath = Path.Combine(val, oldfile.ImageUrl.ToString());
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath); 
                        }
                    }
                    
                    string newfileName = Guid.NewGuid().ToString() + "_"  + fileName ;

                    var physicalPath = _evn.ContentRootPath + "/Content/images/products/" +  newfileName;

                    using(var stream = new FileStream(physicalPath,FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }

                    return Ok(newfileName);
                }
                return BadRequest(new ApiResponse(400, "Problem adding photo product"));
            }
            catch (System.Exception)
            {
                return BadRequest(new ApiResponse(400, "Problem adding photo product"));
            }
        }
    }
}