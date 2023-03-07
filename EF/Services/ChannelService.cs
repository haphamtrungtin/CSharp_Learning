using System;
using System.Text.RegularExpressions;
using EF.Data;

namespace EF.Services
{
    public class ChannelService
    {   
        internal static DashboardItem GetTotalPrice(Func<OrderInfo, bool> predicate)
        {
            using OMSContext context = new();
            List<DashboardItem> result = context.OrderInfoes
               .Where(predicate)
              .GroupBy(o => o.OrderedAt)
              .Select(g => new DashboardItem { Text = g.ToString(), Value = g.Sum(order => order.TotalPrice) })
              .ToList();
            return new DashboardItem() { Value = context.OrderInfoes.Where(predicate).Sum(o => o.TotalPrice), List = result };
        }
      
    }
}