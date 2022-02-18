using NLayerNet6.Core.Dtos;

namespace NLayerNet6.Web.Services
{
    public class CategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseDto<List<CategoryDto>>>("categories");

            return response.Data;
        }
    }
}
