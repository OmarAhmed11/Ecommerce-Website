using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
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
        private readonly IConnectionMultiplexer _redis;
        private readonly IImageManagementService _imageManagementService;
        private readonly UserManager<AppUser> _userManager;
        public IProductRepository ProductRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public IimageRepository ImageRepository { get; }

        public ICustomerCartRepository CustomerCartRepository { get; }

        public IAuth Auth { get; }
        private readonly IEmailService _emailService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IGenerateToken _generateToken;

        public UnitOfWork(AppDbContext context, IImageManagementService imageManagementService, IMapper mapper,
            IConnectionMultiplexer redis, UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken generateToken)
        {
            _context = context;
            _mapper = mapper;
            _redis = redis;
            _imageManagementService = imageManagementService;
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;
            _generateToken = generateToken;
            ProductRepository = new ProductRepository(_context, _mapper, _imageManagementService);
            CategoryRepository = new CategoryRepository(_context);
            ImageRepository = new ImageRepository(_context);
            CustomerCartRepository = new CustomerCartRepository(_redis);
            Auth = new AuthRepository(userManager, emailService, signInManager, generateToken);
        }
    }
}
