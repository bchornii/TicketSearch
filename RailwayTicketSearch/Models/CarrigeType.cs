using Newtonsoft.Json;

namespace RailwayTicketSearch.Models
{
    public class CarrigeType
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("letter")]
        public string Letter { get; set; }

        [JsonProperty("places")]
        public int Places { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}