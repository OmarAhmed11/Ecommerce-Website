using AutoMapper;
using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities.Order;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, AppDbContext context, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
        }
        public async Task<Order> CreateOrderAsync(OrderDTO orderDTO, string BuyerEmail)
        {
            var cart = await _unitOfWork.CustomerCartRepository.getCartAsync(orderDTO.CartId);

            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (var item in cart.CartItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                var orderItem = new OrderItem
                {
                    ProductItemId = product.Id,
                    ProductName = product.Name, 
                    Price = item.Price,
                    Quantity = item.Quantity,
                };
                orderItems.Add(orderItem);
            }

            var deliverMethod = await _context.deliveryMethods.FirstOrDefaultAsync(m => m.Id == orderDTO.DeliveryMethodId);

            decimal subTotal = orderItems.Sum(m => m.Price * m.Quantity);
            var shipping = _mapper.Map<ShippingAddress>(orderDTO.shippingAddress);

            var order = new Order(BuyerEmail, subTotal, shipping, deliverMethod, orderItems);
            await _context.orders.AddAsync(order);
            await _context.SaveChangesAsync();
            await _unitOfWork.CustomerCartRepository.DeleteCartAsync(orderDTO.CartId);
            return order;
        }

        public async Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var orders = await _context.orders.Where(o => o.BuyerEmail == BuyerEmail)
                            .Include(inc => inc.OrderItems).Include(inc => inc.DeliveryMethod)
                            .ToListAsync();
            var result = _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);
            return result;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            return await _context.deliveryMethods.AsNoTracking().ToListAsync();
        }

        public async Task<OrderToReturnDTO> GetOrderByIdAsync(int Id, string BuyerEmail)
        {
            var order = await _context.orders.Where(o => o.Id == Id && o.BuyerEmail == BuyerEmail)
                .Include(inc => inc.OrderItems).Include(inc => inc.DeliveryMethod)
                .FirstOrDefaultAsync();
            var result = _mapper.Map<OrderToReturnDTO>(order);
            return result;
        }
    }
}
