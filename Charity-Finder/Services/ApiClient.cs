using Azure;
using System;
using System.Composition;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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

        public async Task<string> GetFeaturedCharities()
        {
            // API Key
            string apiKey = "610ee8f9-bb17-4a64-97f6-99fb66929a19";

            // URL
            string baseUrl = "https://api.globalgiving.org/api";
            string operation = "/public/projectservice/featured/projects/summary";
            string queryString = $"?api_key={apiKey}";
            string url = baseUrl + operation + queryString;

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
        public async Task<string> GetDataByTheme(string selectedTheme)
        {
            // API Key
            string apiKey = "610ee8f9-bb17-4a64-97f6-99fb66929a19";

            // URL
            string baseUri = "https://api.globalgiving.org/api";
            string operation = $"/public/projectservice/themes/{selectedTheme}";
            string queryString = $"api_key={apiKey}";
            string url = $"{baseUri}{operation}/projects/active/summary?api_key={apiKey}";

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


        public async Task<string> GetDataBySearch(string selectedRegion, string selectedTheme = "")
        {
            selectedRegion = selectedRegion.Replace(" ", "+");
            // API Key
            string apiKey = "610ee8f9-bb17-4a64-97f6-99fb66929a19";

            // URL
            string baseUri = "https://api.globalgiving.org/api";
            string operation = $"/public/services/search/projects";
            string queryString = $"?api_key={apiKey}&q={selectedRegion}+{selectedTheme}";
            string url = $"{baseUri}{operation}{queryString}";
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

        public async Task<string> GetAnyData()
        {
            string[] themes = {
                "animals",
                "children",
                "climate",
                "democ",
                "disaster",
                "ecdev",
                "edu",
                "env",
                "gender",
                "health",
                "human",
                "rights",
                "sport",
                "tech",
                "hunger",
                "art",
                "lgbtq",
                "covid-19",
                "water",
                "disability",
                "endabuse",
                "mentalhealth",
                "justice",
                "refugee",
                "reproductive",
                "housing",
                "agriculture",
                "wildlife"
            };

            // randomly select theme to search charities for
            Random random = new Random();
            string selectedTheme = themes[random.Next(0, themes.Length)];

            // API Key
            string apiKey = "610ee8f9-bb17-4a64-97f6-99fb66929a19";

            // URL
            string baseUri = "https://api.globalgiving.org/api";
            string operation = $"/public/projectservice/themes/{selectedTheme}";
            string queryString = $"api_key={apiKey}";
            string url = $"{baseUri}{operation}/projects/active/summary?api_key={apiKey}";

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
