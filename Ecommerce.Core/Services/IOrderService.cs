using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(OrderDTO orderDTO, string BuyerEmail);
        Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail);
        Task<OrderToReturnDTO> GetOrderByIdAsync(int Id, string BuyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
    }
}
