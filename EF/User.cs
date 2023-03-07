using Microsoft.IdentityModel.Tokens;

namespace EF
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Conversation> Conversations { get; set; } = new List<Conversation>();

        
    }

}