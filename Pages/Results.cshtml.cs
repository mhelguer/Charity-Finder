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
            CharitiesObj = TempData["CharitiesObj"] as List<Charity>;

        }
    }
}
