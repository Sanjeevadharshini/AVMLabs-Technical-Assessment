using System.Net.Http.Json;
using AVMLabs.Mvc.Models.Common;

namespace AVMLabs.Mvc.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResponse<T>?> GetAsync<T>(string endpoint)
        {
            var client = _httpClientFactory.CreateClient("AVMLabsApi");
            return await client.GetFromJsonAsync<ApiResponse<T>>(endpoint);
        }

        public async Task<ApiResponse<T>?> PostAsync<T>(string endpoint, object data)
        {
            var client = _httpClientFactory.CreateClient("AVMLabsApi");
            var response = await client.PostAsJsonAsync(endpoint, data);
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        }
    }
}