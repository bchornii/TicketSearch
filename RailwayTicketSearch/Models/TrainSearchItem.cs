using System.Collections.Generic;
using Newtonsoft.Json;

namespace RailwayTicketSearch.Models
{
    public class TrainSearchItem
    {
        [JsonProperty("allowBooking")]
        public bool AllowBooking { get; set; }

        [JsonProperty("allowRoundtrip")]
        public bool AllowRoundtrip { get; set; }

        [JsonProperty("num")]
        public string TrainNumber { get; set; }

        [JsonProperty("from")]
        public RailwayStantion From { get; set; }

        [JsonProperty("to")]
        public RailwayStantion Till { get; set; }

        [JsonProperty("travelTime")]
        public string TravelTime { get; set; }

        [JsonProperty("types")]
        public List<CarrigeType> AvailiableCarrigeTypes { get; set; }
    }
}