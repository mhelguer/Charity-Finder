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
            // FIXME: response not successful when using selectedTheme variable instead of hardcoding org theme edu
            // API Key
            string apiKey = "610ee8f9-bb17-4a64-97f6-99fb66929a19";

            // URL
            string baseUri = "https://api.globalgiving.org/api";
            string operation = $"/public/projectservice/themes/{selectedTheme}";
            string queryString = $"api_key={apiKey}";
            string url = $"{baseUri}{operation}/projects/active/summary?api_key={apiKey}";
            Console.WriteLine("url: "+url);

            HttpResponseMessage response = await _httpClient.GetAsync(url);
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
