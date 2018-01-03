using System.Configuration;

namespace RailwayTicketSearch
{
    public class AppSettings
    {
        public string TwilioFrom { get; set; }
        public string TwilioTo { get; set; }
        public string StantionIdFrom { get; set; }
        public string StantionIdTo { get; set; }
        public string StationFrom { get; set; }
        public string StationTo { get; set; }
        public string DateDep { get; set; }
        public string TimeDep { get; set; }
        public double MaxTravelTime { get; set; }
        public string[] Types { get; set; }
        public bool EnableSmsNotifications { get; set; }
        public bool EnableEmailNotifications { get; set; }

        public AppSettings GetAppSettings()
        {
            return new AppSettings
            {
                DateDep = ConfigurationManager.AppSettings["DateDep"],
                TimeDep = ConfigurationManager.AppSettings["TimeDep"],
                StantionIdFrom = ConfigurationManager.AppSettings["StantionIdFrom"],
                StantionIdTo = ConfigurationManager.AppSettings["StantionIdTo"],
                StationFrom = ConfigurationManager.AppSettings["StationFrom"],
                StationTo = ConfigurationManager.AppSettings["StationTo"],
                TwilioFrom = ConfigurationManager.AppSettings["TwilioFrom"],
                TwilioTo = ConfigurationManager.AppSettings["TwilioTo"],
                MaxTravelTime = double.Parse(ConfigurationManager.AppSettings["MaxTravelTime"]),
                Types = ConfigurationManager.AppSettings["Types"]?.Split(','),
                EnableSmsNotifications = bool.Parse(ConfigurationManager.AppSettings["EnableSmsNotifications"]),
                EnableEmailNotifications = bool.Parse(ConfigurationManager.AppSettings["EnableEmailNotifications"])
            };
        }
    }
}