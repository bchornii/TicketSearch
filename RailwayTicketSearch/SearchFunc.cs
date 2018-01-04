using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Twilio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RailwayTicketSearch.Extensions;
using RailwayTicketSearch.Infrastructure;
using RailwayTicketSearch.Models;

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
            var trains = Convertes.ToTrainSearchItems(response);           
            
            // Apply filters                        
            var filteredTrains = trains
                .Where(item => Convertes.TravelTimeToHours(item.TravelTime) <= config.MaxTravelTime)
                .Where(item => item.AvailiableCarrigeTypes.Any(ct => config.Types.Contains(ct.Letter)))                
                .OrderBy(item => Convertes.TravelTimeToHours(item.TravelTime))
                .ToList();            

            // Generate summary                    
            var summary = CreateSummary(filteredTrains);
            log.Info($"Ticket found short info: {summary}");
             
            // Send email            
            if (config.EnableEmailNotifications)
            {
                var report = CreateReport(filteredTrains);
                var smtpClient = new SmtpUserClient();
                await smtpClient.SendEmail("Tickets found", report);
            }

            // Send SMS
            if (config.EnableSmsNotifications)
            {                
                var message = new SMSMessage
                {
                    Body = summary,
                    From = ConfigurationManager.AppSettings["TwilioFrom"],
                    To = ConfigurationManager.AppSettings["TwilioTo"]
                };
                return message;  
            }

            return null;          
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
            var result = await client
                .FormUrlEncodedPostAsString("https://booking.uz.gov.ua/purchase/search/", nvc);
            return result;
        }

        private static string CreateReport(IReadOnlyCollection<TrainSearchItem> trainSearchItems)
        {
            var sb = new StringBuilder();
            var summary = CreateSummary(trainSearchItems);
            sb.AppendLine($"Summary: {summary}");
            sb.AppendLine(new string('-', 80));

            foreach (var train in trainSearchItems)
            {
                sb.AppendLine($"Train number: {train.TrainNumber}");
                sb.AppendLine($"Allow booking: {train.AllowBooking}");
                sb.AppendLine($"Travel time : {train.TravelTime}");
                sb.AppendLine($"From: {train.From.StationName} Till: {train.Till.StationName}");
                sb.AppendLine($"Arrival to start: {train.From.ArrivalDateTime}");
                sb.AppendLine($"Arrival to finish: {train.Till.ArrivalDateTime}");

                var places = string.Join(",",
                    train.AvailiableCarrigeTypes.Select(ct => $"{ct.Title}={ct.Places}"));
                sb.AppendLine($"Availiable places: {places}");
                sb.AppendLine(new string('-', 80));
            }
            return sb.ToString();
        }

        private static string CreateSummary(IReadOnlyCollection<TrainSearchItem> trainSearchItems)
        {
            var allCarrigeTypes = trainSearchItems
                .SelectMany(fct => fct.AvailiableCarrigeTypes)
                .GroupByCarrigeType();

            var types = new List<KeyValuePair<string, int>>();
            types.AddRange(allCarrigeTypes.Select(group => new KeyValuePair<string, int>(group.Type, group.Total)));

            var summary = $"Total trains={trainSearchItems.Count};" +
                          $"Types= {string.Join(",", types.Select(t => $"{t.Key}:{t.Value}"))}";

            return summary;
        }
    }
}
