using System;
using Newtonsoft.Json;

namespace RailwayTicketSearch.Models
{
    public class RailwayStantion
    {
        [JsonProperty("time")]
        public DateTime ArrivalTime { get; set; }

        [JsonProperty("srcDate")]
        public DateTime ArrivalDateTime { get; set; }

        [JsonProperty("station")]
        public string StationName { get; set; }
    }
}