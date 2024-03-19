using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using CharityFinder.Models;
using Newtonsoft.Json.Linq;

namespace CharityFinder.Services
{
    public class CharityService
    {
        private readonly HttpClient _httpClient;

        public CharityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Charity>> GetCharitiesAsync(string apiResponse)
        {                   
            //Console.WriteLine("in GetCharitiesAsync, apiResponse: "+ apiResponse);

            Console.WriteLine("apiResponse: "+apiResponse);
            try
            {
                JToken.Parse(apiResponse);
                Console.WriteLine("apiREsponse parsed as json object");
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine("Error parsing apiResponse: " + ex.Message);
            }


            try
            {
                List<Charity> charities = ParseApiResponse(apiResponse);
                foreach (Charity charity in charities)
                {
                    Console.WriteLine($"Name: {charity.Name}");
                    Console.WriteLine($"Summary: {charity.Summary}");
                    Console.WriteLine($"Countries Served: {string.Join(", ", charity.CountriesServed)}");
                    Console.WriteLine($"Home Country: {charity.HomeCountry}");
                    Console.WriteLine($"URL: {charity.Url}");
                    Console.WriteLine();
                }
                Console.WriteLine("finished charities: " + charities);
                return charities;
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                return null;
            }

        }

        public List<Charity> ParseApiResponse(string apiResponse)
        {
            // deserialize json string into list of Charity objects
            List<Charity> charities = JsonConvert.DeserializeObject<List<Charity>>(apiResponse);

            return charities;
        }
    }
}
