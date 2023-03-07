using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF
{
    public class Emoji
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Shortcut { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Unicode { get; set; }
        public User Sender { get; set; }
        public int Quantity { get; set; }

       

    }
}
