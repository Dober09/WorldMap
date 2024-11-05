using System.Text.Json.Serialization;

namespace WorldMap.Models
{
    public class CountryModel
    {
        [JsonPropertyName("name")]
        public Name Name { get; set; }


        [JsonPropertyName("capital")]
        public List<string> Capital { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }



        [JsonPropertyName("Flags")]
        public Flags Flags { get; set; }


        [JsonPropertyName("languages")]
        public Dictionary<string, string> Languages { get; set; }


        [JsonPropertyName("population")]
         public int Population { get; set; } 
    }

    public class Name
    {
        [JsonPropertyName("commom")]
        public string Common { get; set; }

        [JsonPropertyName("official")]
        public string Offical { get; set; }
    }

    public class Flags
    {
        [JsonPropertyName("png")]
        public string Png { get; set; }
    }


}
