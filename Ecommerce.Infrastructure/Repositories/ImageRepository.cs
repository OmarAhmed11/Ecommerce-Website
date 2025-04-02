using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ImageRepository : GenericRepository<Image>, IimageRepository
    {
        public ImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
