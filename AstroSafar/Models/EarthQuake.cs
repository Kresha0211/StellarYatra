using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AstroSafar.Models
{
    
        public class Earthquake
        {
        [Key]
        public int Id { get; set; }
            public string Location { get; set; }
            public double Magnitude { get; set; }
            public DateTime Time { get; set; }
        }

    public class EarthquakeApiResponse
    {
        [JsonProperty("earthquakes")]
        public List<Earthquake> Earthquakes { get; set; }
    }
}

