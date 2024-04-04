using CharityFinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace CharityFinder.Pages
{
    public class ResultsModel : PageModel
    {
        public List<Charity> CharitiesObj { get; set; }
        public void OnGet()
        {
            var jsonString = HttpContext.Session.GetString("CharitiesObj");
            if (!string.IsNullOrEmpty(jsonString))
            {
                CharitiesObj = JsonConvert.DeserializeObject<List<Charity>>(jsonString);
                ViewData["CharitiesObj"] = CharitiesObj;
            }
        }
    }
}
