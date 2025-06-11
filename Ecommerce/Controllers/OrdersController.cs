using Ecommerce.Core.DTOs;
using Ecommerce.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("create-order")]
        public async Task<IActionResult> create(OrderDTO orderDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var order = await _orderService.CreateOrderAsync(orderDTO,email);
            return Ok(order);
        }
        [HttpGet("get-user-orders")]
        public async Task<IActionResult> getUserOrders()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var orders = await _orderService.GetAllOrdersForUserAsync(email);
            return Ok(orders);
        }
        [HttpGet("get-user-orders-byId/{id}")]
        public async Task<IActionResult> getOrderById(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var orders = await _orderService.GetOrderByIdAsync(id, email);
            return Ok(orders);
        }
        [HttpGet("get-delivery-methods")]
        public async Task<IActionResult> getDeliveryMehtods(int id)
        {
            return Ok(await _orderService.GetDeliveryMethodAsync());
        }
    }
}
