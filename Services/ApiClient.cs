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

        public async Task<string> GetDataByCountry(string selectedCountry)
        {
            // API Key
            string apiKey = "610ee8f9-bb17-4a64-97f6-99fb66929a19";

            // URL
            string baseUri = "https://api.globalgiving.org/api";
            string operation = $"/public/projectservice/countries/{selectedCountry}";
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

        public async Task<string> GetAnyData()
        {
            // themes
            //FIXME: replace theme Names with Id's(strings with no spaces)
            string[] themes = {
                "Animal Welfare",
                "Child Protection",
                "Climate Action",
                "Peace and Reconciliation",
                "Disaster Response",
                "Economic Growth",
                "Education",
                "Ecosystem Restoration",
                "Gender Equality",
                "Physical Health",
                "Ending Human Trafficking",
                "Justice and Human Rights",
                "Sport",
                "Digital Literacy",
                "Food Security",
                "Arts and Culture",
                "LGBTQIA+ Equality",
                "COVID-19",
                "Clean Water",
                "Disability Rights",
                "Ending Abuse",
                "Mental Health",
                "Racial Justice",
                "Refugee Rights",
                "Reproductive Health",
                "Safe Housing",
                "Sustainable Agriculture",
                "Wildlife Conservation"
            };

            Random random = new Random();
            string selectedTheme = themes[random.Next(0, themes.Length)];

            Console.WriteLine(selectedTheme);
            // API Key
            string apiKey = "610ee8f9-bb17-4a64-97f6-99fb66929a19";

            // URL
            string baseUri = "https://api.globalgiving.org/api";
            string operation = $"/public/projectservice/themes/{selectedTheme}";
            string queryString = $"api_key={apiKey}";
            string url = $"{baseUri}{operation}/projects/active/summary?api_key={apiKey}";
            Console.WriteLine(url);

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();

            }
            else
            {
                Console.WriteLine("HELLO");
                return $"Error: {response.StatusCode}";
            }
        }
    }
}
