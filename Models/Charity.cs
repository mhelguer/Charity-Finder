using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CharityFinder.Models
{
    public class Charity
    {
        public string Name { get; set; }

        public string Summary { get; set; }

        public List<string> CountriesServed  { get; set; }

        public string HomeCountry { get; set; }

        public string Url { get; set; }
    }
}
