using System;
using System.Collections.Generic;
using System.Linq;
using RailwayTicketSearch.Infrastructure;
using RailwayTicketSearch.Models;

namespace RailwayTicketSearch.Extensions
{
    public static class TicketItemExtensions
    {
        public static IEnumerable<CarrigeTypeAmount> GroupByCarrigeType(this IEnumerable<CarrigeType> source)
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
            return result;
        }
    }
}