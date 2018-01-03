using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Twilio;
using System.Linq;
using System.Threading.Tasks;
using RailwayTicketSearch.Extensions;
using RailwayTicketSearch.Infrastructure;

namespace RailwayTicketSearch
{
    public static class SearchFunc
    {
        [FunctionName("SearchFunc")]
        [return: TwilioSms(
            AccountSidSetting = "TwilioAccountSid",
            AuthTokenSetting = "TwilioAuthToken")]
        public static async Task<SMSMessage> Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");       

            var config = new AppSettings().GetAppSettings();
            var response = await GetAvailiableTrains(config);
            if (string.IsNullOrEmpty(response))
            {
                return null;
            }            

            // Parse response json string to list of ticket search items
            var ticketSearchItems = Convertes.ConvertStringToTicketSearchItems(response);           
            
            // Apply filters
            var maxTravelTime = double.Parse(config.MaxTravelTime);
            var carrigeTypes = config.Types;            
            var filteredTicketItems = ticketSearchItems.ApplyFilters(maxTravelTime, carrigeTypes);            
            if (filteredTicketItems.Count < 5)
            {
                return null;
            }

            // Group by carrige type        
            var grouppedCarrigeTypes = filteredTicketItems
                .SelectMany(fct => fct.AvailiableCarrigeTypes)
                .GroupByCarrigeType();
            var types = new List<KeyValuePair<string, int>>();            
            types.AddRange(grouppedCarrigeTypes.Select(group => new KeyValuePair<string, int>(group.Type, group.Total)));

            // Response SMS text
            var text = $"Total trains={filteredTicketItems.Count};" +
                       $"Types= {string.Join(",", types.Select(t => $"{t.Key}:{t.Value}"))}";
            log.Info($"Text to be sent: {text}");
            var message = new SMSMessage
            {
                Body = text,
                From = ConfigurationManager.AppSettings["TwilioFrom"],
                To = ConfigurationManager.AppSettings["TwilioTo"]
            };
            return message;            
        }       

        private static async Task<string> GetAvailiableTrains(AppSettings config)
        {
            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("station_id_from", config.StantionIdFrom),
                new KeyValuePair<string, string>("station_id_till", config.StantionIdTo),
                new KeyValuePair<string, string>("station_from", config.StationFrom),
                new KeyValuePair<string, string>("station_till", config.StationTo),
                new KeyValuePair<string, string>("date_dep", config.DateDep),
                new KeyValuePair<string, string>("time_dep", config.TimeDep),
                new KeyValuePair<string, string>("time_dep_till", ""),
                new KeyValuePair<string, string>("another_ec", "0"),
                new KeyValuePair<string, string>("search", "")
            };

            var client = new HttpUserClient();
            var result = await client.FormUrlEncodedPostAsString("https://booking.uz.gov.ua/purchase/search/", nvc);
            return result;
        }
    }
}
