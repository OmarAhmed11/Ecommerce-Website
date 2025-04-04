using AutoMapper;
using Ecommerce.Core.DTOs;
using Ecommerce.Core.Interfaces;
using Ecommerce.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> get()
        {
            try
            {
                var products = await unitOfWork.ProductRepository.GetAllAsync(x => x.Category, x => x.Images);
                var result = mapper.Map<List<ProductDTO>>(products);
                if (products == null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> getById(int id)
        {
            try
            {
                var product = await unitOfWork.ProductRepository.GetByIdAsync(id,
                    x => x.Category, x => x.Images);

                var result = mapper.Map<ProductDTO>(product);
                if (result == null)
                {
                    return BadRequest(new ResponseAPI(400, $"No Product Found with Id = {id}"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> add(AddProductDTO addProduct)
        {
            try
            {
                await unitOfWork.ProductRepository.AddAsync(addProduct);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> update(UpdateProductDTO updateProductDTO)
        {
            try
            {
                await unitOfWork.ProductRepository.UpdateAsync(updateProductDTO);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
                var product = await unitOfWork.ProductRepository.GetByIdAsync(id, x => x.Category, x => x.Images);
                await unitOfWork.ProductRepository.DeleteAsync(product);
                return Ok(new ResponseAPI(200));
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
    }
}
