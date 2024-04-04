using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using CharityFinder.Models;

namespace CharityFinder.Services
{
    public class CharityService
    {
        private readonly HttpClient _httpClient;

        public CharityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Charity> GetCharities(string apiResponse)
        {
            List<Charity> charities = new List<Charity>();

            // deserialize xml string into list of Charity objects
            using (StringReader reader = new StringReader(apiResponse))
            {
                XDocument doc = XDocument.Load(reader);

                foreach (XElement projectElement in doc.Descendants("project"))
                {
                    Charity charity = new Charity();

                    // getting Name
                    charity.Name = (string)projectElement.Element("organization")?.Element("name");

                    // Summary
                    charity.Summary = (string)projectElement.Element("summary");

                    // HomeCountry
                    charity.HomeCountry = (string)projectElement.Element("organization")
                        ?.Element("country")
                        ?.Element("name");

                    // Url
                    charity.Url = (string)projectElement.Element("organization")?.Element("url");

                    // add charity to list of charities
                    charities.Add(charity);

                    // see output
                    // Console.WriteLine("adding charity " + charity.Name);
                }
            }

            return charities;

        }
    }
}
