using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class CartController : BaseController
    {
        public CartController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpGet("get-cart/{id}")]
        public async Task<IActionResult> get(string id)
        {
            var result = await unitOfWork.CustomerCartRepository.getCartAsync(id);
            if (result == null) {
                return Ok(new CustomerCart());
            }
            return Ok(result);
        }
        [HttpPost("update-cart")]
        public async Task<IActionResult> add(CustomerCart cart)
        {
            var _cart = await unitOfWork.CustomerCartRepository.UpdateCartAsync(cart);
            return Ok(_cart);
        }
        [HttpDelete("delete-cart/{id}")]
        public async Task<IActionResult> delete(string id)
        {
            var result = await unitOfWork.CustomerCartRepository.DeleteCartAsync(id);
            return result ? Ok(new ResponseAPI(200, "Item deleted")) : BadRequest(new ResponseAPI(400));
        }

    }
}
