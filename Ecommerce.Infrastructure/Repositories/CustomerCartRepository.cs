using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class CustomerCartRepository : ICustomerCartRepository
    {
        public readonly IDatabase _database;
        public CustomerCartRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public Task<bool> DeleteCartAsync(string id)
        {
            return _database.KeyDeleteAsync(id);

        }

        public async Task<CustomerCart> getCartAsync(string id)
        {
            var result = await  _database.StringGetAsync(id);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonSerializer.Deserialize<CustomerCart>(result);
            }
            return null;
        }

        public async Task<CustomerCart> UpdateCartAsync(CustomerCart cart)
        {
            var _cart = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(3));
            if (_cart) {
                return await getCartAsync(cart.Id);
            }
            return null;
        }
    }
}
