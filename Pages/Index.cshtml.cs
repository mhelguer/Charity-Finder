using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using CharityFinder.Models;
using CharityFinder.Services;

namespace CharityFinder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string Data { get; set; }

        public ThemeModel ThemeModelObj { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        private readonly ApiClient _apiClient;

        [BindProperty]
        public string SelectedTheme { get; set; }


        public async Task OnPost()
        {
            string selectedTheme = SelectedTheme;
            //FIXME: selectedTheme is correct, but _apiClient is null
            Console.WriteLine($"OnPost: {_apiClient}");

            var result = await _apiClient.GetData(selectedTheme);

            Console.WriteLine(result);
        }

        public async Task OnGetAsync()
        {
            Data = "some data";
            await InitializeThemeModel();

            // Ensure ThemeModelObj is not null before accessing its properties
            if (ThemeModelObj != null)
            {
                foreach (var theme in ThemeModelObj.Themes.Theme)
                {
                    Console.WriteLine($"Theme ID: {theme.Id}, Name: {theme.Name}");
                }
            }
            Console.WriteLine("HELLO");

            // Pass ThemeModelObj to the view

            ViewData["ThemeModelObj"] = ThemeModelObj;

        }


        private async Task InitializeThemeModel()
        {
            // API Key
            string apiKey = "610ee8f9-bb17-4a64-97f6-99fb66929a19";

            // URL
            string baseUri = "https://api.globalgiving.org/api";
            string operation = "/public/projectservice/themes";
            string queryString = $"api_key={apiKey}";
            string url = $"{baseUri}{operation}?{queryString}";

            // Create HttpClient
            using (HttpClient client = new HttpClient())
            {
                // Set Accept header
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                // Make GET request
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        // Log the JSON content for debugging
                        Console.WriteLine(jsonContent);

                        ThemeModelObj = JsonConvert.DeserializeObject<ThemeModel>(jsonContent);
                        Console.WriteLine("HELLO");

                        //foreach (var theme in ThemeModelObj.Themes.Theme)
                        //{
                        //    Console.WriteLine(theme.Name);
                        //}
                    }
                    catch (JsonReaderException ex)
                    {
                        // Log the exception for debugging
                        Console.WriteLine(ex);
                        ThemeModelObj = null;
                    }
                }
                else
                {
                    ThemeModelObj = null;
                }
            }
        }
        //private static async Task Main(IndexModel indexModel)
        //{
        //    await indexModel.InitializeThemeModel();

        //}

    }
}

