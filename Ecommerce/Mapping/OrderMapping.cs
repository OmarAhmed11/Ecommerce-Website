using AutoMapper;
using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities.Order;

namespace Ecommerce.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        { 
            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s=> s.DeliveryMethod.Name)).ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<ShippingAddress, ShippingAddressDTO>().ReverseMap();
        }
    }
}
