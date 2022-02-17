using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayerNet6.Core.Dtos;
using NLayerNet6.Core.Models;
using NLayerNet6.Core.Repositories;
using NLayerNet6.Core.Services;
using NLayerNet6.Core.UnitOfWorks;
using NLayerNet6.Service.Exceptions;
using System.Linq.Expressions;

namespace NLayerNet6.Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string ProductCacheKey = "productCacheKey";
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductServiceWithCaching(IProductRepository productRepository, IMemoryCache memoryCache, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productRepository = productRepository;
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            if (!_memoryCache.TryGetValue(ProductCacheKey, out _))
            {
                //_memoryCache.Set(ProductCacheKey, _productRepository.GetAll().ToList());
                _memoryCache.Set(ProductCacheKey, _productRepository.GetProductWithCategory().Result);
            }
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _productRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _productRepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(ProductCacheKey).Any(expression.Compile()));
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(ProductCacheKey));
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var product = _memoryCache.Get<List<Product>>(ProductCacheKey).FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                throw new NotFoundException($"{typeof(Product).Name}({id}) does not exist");
            }

            return Task.FromResult(product);
        }

        public Task<List<ProductWithCategoryDto>> GetProductWithCategory()
        {
            //var products = await _productRepository.GetProductWithCategory();
            //var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            //return ResponseDto<List<ProductWithCategoryDto>>.Success(200, productsDto);

            var products = _memoryCache.Get<IEnumerable<Product>>(ProductCacheKey);
            var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return Task.FromResult(productsDto);
        }

        public async Task RemoveAsync(Product entity)
        {
            _productRepository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _productRepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _productRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _memoryCache.Get<List<Product>>(ProductCacheKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task CacheAllProductsAsync()
        {
            _memoryCache.Set(ProductCacheKey, await _productRepository.GetAll().ToListAsync());
        }
    }
}
