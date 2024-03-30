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
        [BindProperty]
        public string SelectedRegion { get; set; }

        public async Task<IActionResult> OnPost()
        {
            string selectedTheme = SelectedTheme;
            string selectedRegion = SelectedRegion;

            // search for random projects
            if (selectedTheme == "Any" && selectedRegion == "Any")
            {
                var apiResponse = await _apiClient.GetAnyData();
                CharitiesObj = _charityService.GetCharities(apiResponse);
                TempData["CharitiesObj"] = JsonConvert.SerializeObject(CharitiesObj);
            }
            // search based on only theme
            else if (selectedTheme != "Any" && selectedRegion == "Any")
            {
                var apiResponse = await _apiClient.GetDataByTheme(selectedTheme);   // result is string
                CharitiesObj = _charityService.GetCharities(apiResponse);
                TempData["CharitiesObj"] = JsonConvert.SerializeObject(CharitiesObj);
            }
            // search when only region is chosen, or when theme and region is chosen (same method)
            else
            {
                if (selectedTheme == "Any")
                {
                    var apiResponse = await _apiClient.GetDataBySearch(selectedRegion);
                    CharitiesObj = _charityService.GetCharities(apiResponse);
                    TempData["CharitiesObj"] = JsonConvert.SerializeObject(CharitiesObj);
                }
                else
                {
                    var apiResponse = await _apiClient.GetDataBySearch(selectedRegion, selectedTheme);
                    CharitiesObj = _charityService.GetCharities(apiResponse);
                    TempData["CharitiesObj"] = JsonConvert.SerializeObject(CharitiesObj);
                }
            }

            return RedirectToPage("/Results");

        }

        public async Task OnGetAsync()
        {
            Data = "some data";
            await InitializeThemeModel();

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
                        ThemeModelObj = JsonConvert.DeserializeObject<ThemeModel>(jsonContent);

                    }
                    catch (JsonReaderException ex)
                    {
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

