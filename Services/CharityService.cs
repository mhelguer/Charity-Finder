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
            Console.WriteLine("IN GET CHARITIES");
            List<Charity> charities = new List<Charity>();

            // deserialize xml string into list of Charity objects
            using (StringReader reader = new StringReader(apiResponse))
            {

                XDocument doc = XDocument.Load(reader);

                // TODO: get all themes for each charity to display in their card above description
                foreach (XElement projectElement in doc.Descendants("project"))
                {

                    Charity charity = new Charity();

                    // getting Name
                    charity.Name = (string)projectElement.Element("organization")?.Element("name");

                    // Summary
                    charity.Summary = (string)projectElement.Element("summary");

                    // HomeCountry
                    charity.HomeCountry = (string)projectElement.Element("countries")
                        ?.Element("country")
                        ?.Element("name");


                    // Url
                    charity.Url = (string)projectElement.Element("organization")?.Element("url");

                    // Themes
                    List<string> themes = new List<string>();

                    foreach (XElement themeElement in projectElement.Element("organization").Element("themes").Elements("theme"))
                    {
                        string themeName = (string)themeElement.Element("name");
                        themes.Add(themeName);
                    }
                    charity.RelatedThemes = themes;

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
