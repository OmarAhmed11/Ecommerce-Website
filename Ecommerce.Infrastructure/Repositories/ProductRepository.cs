using AutoMapper;
using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Core.Shared;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;
        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<ReturnProductDTO> GetAllAsync(ProductParams productParams)
        {
            var query = context.Products.Include(p => p.Category).Include(p => p.Images).AsNoTracking();

            // Filtering by Specific String

            // Normal Search
            //if(!string.IsNullOrEmpty(productParams.Search))
            //    query = query
            //            .Where(p => p.Name.ToLower().Contains(productParams.Search.ToLower()) ||
            //            p.Description.ToLower().Contains(productParams.Search.ToLower()));

            //Smart Search to get the product that contain any word sent in the search string
            if (!string.IsNullOrEmpty(productParams.Search))
            {
                var searchWords = productParams.Search.Split(' ');
                query = query.Where(p => searchWords.All(word =>

                    p.Name.ToLower().Contains(word.ToLower()) ||
                    p.Description.ToLower().Contains(word.ToLower())
                ));
            }

            // Filtering by CategoryId
            if (productParams.CategoryId.HasValue)
                query = query.Where(p =>  p.CategoryId == productParams.CategoryId);
            
            if(!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PriceAsc" => query.OrderBy(p => p.NewPrice),
                    "PriceDsc" => query.OrderByDescending(p => p.NewPrice),
                    _ => query.OrderBy(p => p.Name),
                };
            }

            ReturnProductDTO returnProductDTO = new ReturnProductDTO();
            returnProductDTO.TotalCount = query.Count(); 

            productParams.PageNumber = productParams.PageNumber > 0 ? productParams.PageNumber : 1;
            productParams.PageSize = productParams.PageSize > 0 ? productParams.PageSize : 6;
            query = query.Skip((productParams.PageSize) * (productParams.PageNumber - 1)).Take(productParams.PageSize);
            returnProductDTO.Products = mapper.Map<List<ProductDTO>>(query);
            return returnProductDTO;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO == null) return false;
            var product = mapper.Map<Product>(productDTO);
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            var ImagePath = await imageManagementService.AddImageAsync(productDTO.Images, productDTO.Name);
            var Image = ImagePath.Select(path => new Image
            {
                ImageName = path,
                ProductId = product.Id

            }).ToList();
            await context.Images.AddRangeAsync(Image);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if(updateProductDTO is null)
            {
                return false;
            }
            var FindProduct = await context.Products.Include(m => m.Category)
                .Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == updateProductDTO.Id);
            if (FindProduct is null)
            {
                return false;
            }
            mapper.Map(updateProductDTO, FindProduct);
            var FindImage = await context.Images.Where(m => m.ProductId == updateProductDTO.Id).ToListAsync();
            foreach (var item in FindImage)
            {
                    imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Images.RemoveRange(FindImage);
            var ImagePath = await imageManagementService.AddImageAsync(updateProductDTO.Images, updateProductDTO.Name);
            var Image = ImagePath.Select(path => new Image
            {
                ImageName= path,
                ProductId=updateProductDTO.Id
            }).ToList();
            await context.Images.AddRangeAsync(Image);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(Product product)
        {
            var image = await context.Images.Where(m => m.ProductId == product.Id).ToListAsync();

            foreach (var item in image)
            {
                imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();

        }
    }
}
