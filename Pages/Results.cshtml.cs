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

            var charitiesJson = TempData["CharitiesObj"] as string;
            Console.WriteLine(charitiesJson);
            CharitiesObj = JsonConvert.DeserializeObject<List<Charity>>(charitiesJson);
            ViewData["CharitiesObj"] = CharitiesObj;


        }
    }
}
