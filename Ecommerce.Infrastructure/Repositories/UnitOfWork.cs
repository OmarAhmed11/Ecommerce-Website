using AutoMapper;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;
        public IProductRepository ProductRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public IimageRepository ImageRepository { get; }
        public UnitOfWork(AppDbContext context, IImageManagementService imageManagementService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
            ProductRepository = new ProductRepository(_context,_mapper,_imageManagementService);
            CategoryRepository = new CategoryRepository(_context);
            ImageRepository = new ImageRepository(_context);
        }
    }
}
