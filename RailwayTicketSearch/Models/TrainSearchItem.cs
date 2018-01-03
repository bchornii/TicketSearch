using System.Collections.Generic;
using Newtonsoft.Json;

namespace RailwayTicketSearch.Models
{
    public class TrainSearchItem
    {
        [JsonProperty("allow_booking")]
        public bool AllowBooking { get; set; }

        [JsonProperty("allow_roundtrip")]
        public bool AllowRoundtrip { get; set; }

        [JsonProperty("num")]
        public string TrainNumber { get; set; }

        [JsonProperty("from")]
        public RailwayStantion From { get; set; }

        [JsonProperty("till")]
        public RailwayStantion Till { get; set; }

        [JsonProperty("travel_time")]
        public string TravelTime { get; set; }

        [JsonProperty("types")]
        public List<CarrigeType> AvailiableCarrigeTypes { get; set; }
    }
}