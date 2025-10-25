using B_M.Data.Repositories;
using B_M.Models.Entities;
using System;

namespace B_M.Data
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext _context;
        private bool _disposed = false;

        // Repositories
        private UserRepository _userRepository;
        private BrandRepository _brandRepository;
        private ProductRepository _productRepository;
        private CommunityPostRepository _communityPostRepository;
        private SocialPostRepository _socialPostRepository;
        private TradePostRepository _tradePostRepository;
        private MilkDonationPostRepository _milkDonationPostRepository;

        public UnitOfWork()
        {
            _context = new ApplicationDbContext();
        }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        // Repository properties
        public UserRepository Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
            }
        }

        public BrandRepository Brands
        {
            get
            {
                if (_brandRepository == null)
                    _brandRepository = new BrandRepository(_context);
                return _brandRepository;
            }
        }

        public ProductRepository Products
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository(_context);
                return _productRepository;
            }
        }

        public CommunityPostRepository CommunityPosts
        {
            get
            {
                if (_communityPostRepository == null)
                    _communityPostRepository = new CommunityPostRepository(_context);
                return _communityPostRepository;
            }
        }

        public SocialPostRepository SocialPosts
        {
            get
            {
                if (_socialPostRepository == null)
                    _socialPostRepository = new SocialPostRepository(_context);
                return _socialPostRepository;
            }
        }

        public TradePostRepository TradePosts
        {
            get
            {
                if (_tradePostRepository == null)
                    _tradePostRepository = new TradePostRepository(_context);
                return _tradePostRepository;
            }
        }

        public MilkDonationPostRepository MilkDonationPosts
        {
            get
            {
                if (_milkDonationPostRepository == null)
                    _milkDonationPostRepository = new MilkDonationPostRepository(_context);
                return _milkDonationPostRepository;
            }
        }

        // Save changes
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        // Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}