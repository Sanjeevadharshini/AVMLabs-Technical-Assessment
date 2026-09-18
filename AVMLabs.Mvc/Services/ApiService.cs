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
            try
            {
                var client = _httpClientFactory.CreateClient("AVMLabsApi");
                var response = await client.GetAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public async Task<ApiResponse<T>?> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AVMLabsApi");
                var response = await client.PostAsJsonAsync(endpoint, data);

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}