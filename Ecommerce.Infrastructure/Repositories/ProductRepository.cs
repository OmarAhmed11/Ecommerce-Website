using AutoMapper;
using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities.Product;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
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
