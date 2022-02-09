using AutoMapper;
using NLayerNet6.Core.Dtos;
using NLayerNet6.Core.Models;
using NLayerNet6.Core.Repositories;
using NLayerNet6.Core.Services;
using NLayerNet6.Core.UnitOfWorks;

namespace NLayerNet6.Service.Services
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(IRepository<Category> repository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto<CategoryWithProductsDto>> GetCategoryByIdWithProductsAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdWithProductsAsync(id);
            var categoryDto = _mapper.Map<CategoryWithProductsDto>(category);
            return ResponseDto<CategoryWithProductsDto>.Success(200, categoryDto);
        }
    }
}
