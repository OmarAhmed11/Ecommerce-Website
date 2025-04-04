using AutoMapper;
using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Ecommerce.Helper;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> get()
        {
            try
            {
                var categories = await unitOfWork.CategoryRepository.GetAllAsync();
                if (categories == null) {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(categories);
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
                var category = await unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null) {
                    return BadRequest(new ResponseAPI(400, $"No Category Found with Id = {id}"));
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> add(CategoryDTO categoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDTO);
                await unitOfWork.CategoryRepository.AddAsync(category);
         
                return Ok(new ResponseAPI(200, "Category has beed Added Successfully"));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> update(UpdateCategoryDTO categoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDTO);
                await unitOfWork.CategoryRepository.UpdateAsync(category);
         
                return Ok(new ResponseAPI(200, "Category has beed Updated Successfully"));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> delete(int id)
        {
            try
            {
             
                await unitOfWork.CategoryRepository.DeleteAsync(id);
         
                return Ok(new ResponseAPI(200, "Category has beed Deleted Successfully"));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
