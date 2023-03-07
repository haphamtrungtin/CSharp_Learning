using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF
{
    public class UserEmoji
    {
        public int Id { get; set; }
        public List<Emoji> Emojies { get;set; } = new List<Emoji>();
        public List<User> Users { get;set; } = new List<User>();
        public Message Message { get;set; }

        internal static void CreateEmoji(User user, Message message, Emoji emoji)
        {
            if (user != null)
            {
                message.Emojies.Add(emoji);
            }
            throw new NotImplementedException();
        }
    }
}
