using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF
{
    internal class MessageService
    {
        internal static Message SortEmojiList(Message message)
        {
            //List<Emoji> sortedList = new();
            //if (Emojies.Any(g => g.Quantity > 1))
            //{
            //    sortedList = Emojies.DistinctBy(x => x.Quantity).OrderByDescending(g => g.Quantity).ToList();
            //}
            //else
            //{
            //    sortedList = Emojies.OrderBy(g => g.CreatedAt).ToList();
            //}
            //Emojies = sortedList;
            message.Emojies = message.Emojies
                .Select(x => new Emoji
                {
                    Id = x.Id,
                    Shortcut = x.Shortcut,
                    Category = x.Category,
                    IsDefault = x.IsDefault,
                    CreatedAt = x.CreatedAt,
                    Quantity = message.Emojies.Where(g => g.Unicode.Equals(x.Unicode)).Count(),
                    Unicode = x.Unicode,
                    Name = x.Name,
                })
                .DistinctBy(x => x.Unicode)
                .OrderBy(x => x.CreatedAt)
                .ThenByDescending(x => x.Quantity)
                .ToList();

            return message;
        }

        internal static HashSet<Reactor> ShowListReactor(Message message)
        {
            if (message != null && message.Reactors.Count > 0)
            {
                return message.Reactors;
            }
            return new HashSet<Reactor>();
        }       
    }
}
