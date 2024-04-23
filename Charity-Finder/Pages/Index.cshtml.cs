using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using CharityFinder.Models;
using CharityFinder.Services;
using NuGet.ContentModel;

namespace CharityFinder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApiClient _apiClient;
        private readonly CharityService _charityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;


        public ThemeModel ThemeModelObj { get; set; }
        public List<Charity> CharitiesObj { get; set; }
        public IndexModel(ApiClient apiClient, CharityService charityService, IHttpContextAccessor httpContextAccessor)
        {
            _apiClient = apiClient;
            _charityService = charityService;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public string SelectedTheme { get; set; }
        [BindProperty]
        public string SelectedRegion { get; set; }

        [BindProperty]
        public string SearchString { get; set; }


        public async Task<IActionResult> OnPost()
        {
            if (Request.Form.ContainsKey("SearchString"))
            {
                string searchString = SearchString;

                var apiResponse = await _apiClient.GetDataBySearch(searchString);
                CharitiesObj = _charityService.GetCharities(apiResponse);
                var jsonString = JsonConvert.SerializeObject(CharitiesObj);
                _httpContextAccessor.HttpContext.Session.SetString("CharitiesObj", jsonString);
            }
            else if (Request.Form.ContainsKey("SelectedTheme"))
            {
                string selectedTheme = SelectedTheme;
                string selectedRegion = SelectedRegion;

                // search for random projects
                if (selectedTheme == "Any" && selectedRegion == "Any")
                {
                    // get charities and store as string in session
                    var apiResponse = await _apiClient.GetAnyData();
                    CharitiesObj = _charityService.GetCharities(apiResponse);
                    var jsonString = JsonConvert.SerializeObject(CharitiesObj);
                    _httpContextAccessor.HttpContext.Session.SetString("CharitiesObj", jsonString);
                }
                // search based on only theme
                else if (selectedTheme != "Any" && selectedRegion == "Any")
                {
                    var apiResponse = await _apiClient.GetDataByTheme(selectedTheme);   // result is string
                    CharitiesObj = _charityService.GetCharities(apiResponse);
                    var jsonString = JsonConvert.SerializeObject(CharitiesObj);
                    _httpContextAccessor.HttpContext.Session.SetString("CharitiesObj", jsonString);
                }
                // search when only region is chosen, or when theme and region is chosen (same method)
                else
                {
                    if (selectedTheme == "Any")
                    {
                        var apiResponse = await _apiClient.GetDataBySearch(selectedRegion);
                        CharitiesObj = _charityService.GetCharities(apiResponse);
                        var jsonString = JsonConvert.SerializeObject(CharitiesObj);
                        _httpContextAccessor.HttpContext.Session.SetString("CharitiesObj", jsonString);
                    }
                    else
                    {
                        var apiResponse = await _apiClient.GetDataBySearch(selectedRegion, selectedTheme);
                        CharitiesObj = _charityService.GetCharities(apiResponse);
                        var jsonString = JsonConvert.SerializeObject(CharitiesObj);
                        _httpContextAccessor.HttpContext.Session.SetString("CharitiesObj", jsonString);
                    }
                }
            }
            return RedirectToPage("/Results");

        }

        public async Task OnGetAsync()
        {
            await InitializeThemeModel();
            ViewData["ThemeModelObj"] = ThemeModelObj;
            await InitializeFeaturedCharities();
            ViewData["CharitiesObj"] = CharitiesObj;
        }


        private async Task InitializeThemeModel()
        {
            // API Key
            string apiKey = "610ee8f9-bb17-4a64-97f6-99fb66929a19";

            // URL
            string baseUri = "https://api.globalgiving.org/api";
            string operation = "/public/projectservice/themes";
            string apiString = $"?api_key={apiKey}";
            string url = $"{baseUri}{operation}{apiString}";

            // Create HttpClient
            try
            {
                // Create HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Set Accept header
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    // Make GET request
                    HttpResponseMessage response = await client.GetAsync(url);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching themes: {ex.Message}");
                ThemeModelObj = null;
            }
        }

        private async Task InitializeFeaturedCharities()
        {
            var apiResponse = await _apiClient.GetFeaturedCharities();
            CharitiesObj = _charityService.GetCharities(apiResponse);
            var jsonString = JsonConvert.SerializeObject(CharitiesObj);
        }
    }
}
