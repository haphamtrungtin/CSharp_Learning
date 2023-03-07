using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EF
{
    public class Message
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Content { get; set; }
        public User? Sender { get; set; }
        public bool? IsEdited { get; set; }
        public List<Emoji> Emojies { get; set; } = new List<Emoji>();
        public HashSet<Reactor> Reactors { get; set; } = new HashSet<Reactor>();
        public bool? IsDeleted { get; internal set; }
        public Message? Reply { get; internal set; }
    }

}