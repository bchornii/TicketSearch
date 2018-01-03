using System;
using Newtonsoft.Json;

namespace RailwayTicketSearch.Models
{
    public class RailwayStantion
    {
        [JsonProperty("src_date")]
        public DateTime ArrivalDateTime { get; set; }

        [JsonProperty("station")]
        public string StationName { get; set; }
    }
}