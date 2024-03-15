using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CharityFinder.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GetData(string selectedTheme)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://api.globalgiving.org/api/public/projectservice/themes/edu/projects/active?api_key=610ee8f9-bb17-4a64-97f6-99fb66929a19");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return $"Error: {response.StatusCode}";
            }
        }
    }
}
