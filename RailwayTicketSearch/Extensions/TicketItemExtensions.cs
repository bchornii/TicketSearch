using System;
using System.Collections.Generic;
using System.Linq;
using RailwayTicketSearch.Infrastructure;
using RailwayTicketSearch.Models;

namespace RailwayTicketSearch.Extensions
{
    public static class TicketItemExtensions
    {
        public static IEnumerable<TicketSearchItem> FilterTicketItems(this IEnumerable<TicketSearchItem> source,
            Func<TicketSearchItem, bool> condition)
        {
            return source.Where(condition);
        }

        public static IReadOnlyCollection<CarrigeTypeAmount> GroupByCarrigeType(this IEnumerable<CarrigeType> source)
        {
            var result = 
                from ct in source
                group ct by ct.Letter
                into g
                select new CarrigeTypeAmount
                {
                    Type = g.Key,
                    Total = g.Sum(ct => ct.Places)
                };
            return result.ToList();
        }

        public static IReadOnlyCollection<TicketSearchItem> ApplyFilters(this IEnumerable<TicketSearchItem> source,
            double maxTravelTime, string[] carrigeTypes)
        {
            // filter by carrige type       
            var result = source.FilterTicketItems(item => Convertes.ConvertTravelTimeToHours(item.TravelTime) <= maxTravelTime);

            // filter by carrige type            
            result = result
                .FilterTicketItems(item => item.AvailiableCarrigeTypes.Any(ct => carrigeTypes.Contains(ct.Letter)));

            return result.ToArray();
        }
    }
}