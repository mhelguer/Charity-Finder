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
        private readonly ApiClient _apiClient;
        private readonly CharityService _charityService;
        public string Data { get; set; }

        public ThemeModel ThemeModelObj { get; set; }
        public List<Charity> CharitiesObj { get; set; }
        public IndexModel(ApiClient apiClient, CharityService charityService)
        {
            _apiClient = apiClient;
            _charityService = charityService;
        }

        [BindProperty]
        public string SelectedTheme { get; set; }
        public string SelectedRegion { get; set; }

        public async Task<IActionResult> OnPost()
        {
            string selectedTheme = SelectedTheme;
            string selectedRegion = SelectedRegion;
            if (selectedTheme == "Any")
            {
                var apiResponse = await _apiClient.GetAnyData();
                CharitiesObj = _charityService.GetCharities(apiResponse);
                TempData["CharitiesObj"] = JsonConvert.SerializeObject(CharitiesObj);
            }
            else
            {
                var apiResponse = await _apiClient.GetDataByTheme(selectedTheme);   // result is string
                CharitiesObj = _charityService.GetCharities(apiResponse);
                TempData["CharitiesObj"] = JsonConvert.SerializeObject(CharitiesObj);


            }

            // Console.WriteLine(TempData["CharitiesObj"]); // correct
            Console.WriteLine("REDIRECTING");
            return RedirectToPage("/Results");

        }

        public async Task OnGetAsync()
        {
            Data = "some data";
            await InitializeThemeModel();

            //foreach (var theme in ThemeModelObj.Themes.Theme)
            //{
            //    Console.WriteLine(theme.Id);
            //}

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
                        // Console.WriteLine(jsonContent);

                        ThemeModelObj = JsonConvert.DeserializeObject<ThemeModel>(jsonContent);

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
    }
}

