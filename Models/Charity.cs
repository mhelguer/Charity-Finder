using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace CharityFinder.Models
{
    [XmlRoot("project")]
    public class Charity
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("summary")]
        public string Summary { get; set; }

        [XmlArray("countries")]
        [XmlArrayItem("country")]
        public List<string> CountriesServed { get; set; }

        [XmlElement("contactCountry")]
        public string HomeCountry { get; set; }

        [XmlElement("url")]
        public string Url { get; set; }
    }
}
