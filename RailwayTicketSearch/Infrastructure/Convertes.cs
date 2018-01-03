using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RailwayTicketSearch.Models;

namespace RailwayTicketSearch.Infrastructure
{
    public static class Convertes
    {
        public static double TravelTimeToHours(string travelTime)
        {
            var values = travelTime.Split(':');
            var result = double.Parse(values[0]) + double.Parse(values[1]) / 60;
            return result;
        }

        public static List<TrainSearchItem> ToTrainSearchItems(string response)
        {
            var jobject = JObject.Parse(response);
            return jobject["value"]?.ToObject<List<TrainSearchItem>>();
        }
    }
}