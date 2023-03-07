using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF
{
    internal class UserService
    {
        private readonly static List<User> _users = new List<User>()
        {
             new User(){ Id = 1, Name = "Tin" },
             new User(){ Id = 2, Name = "Trung"},
             new User(){ Id = 3, Name = "Tuan"},
             new User(){ Id = 4, Name = "Tien"}
    };
        internal static List<User> GetUsers()
        {
            return _users;
        }

        internal static Conversation SendMessage(User sender, Message message, User receiver)
        {
            //check if sender conversation has same Id with receiver conversation Id 
            //then add new message to that conversation
            Conversation? currentConversation = sender.Conversations
                                                .Find(c => c.Id.Equals(receiver.Conversations
                                                                       .Find(r => r.Id == c.Id)?.Id));
            if (currentConversation is null)
            {
                //if null
                Conversation conversation = new()
                {
                    Id = 1,
                    Messages = new List<Message>() { message },
                    Users = new List<User>() { sender, receiver }
                };
                sender.Conversations.Add(conversation);
                receiver.Conversations.Add(conversation);
                return conversation;
            }
            else
            {
                currentConversation.Messages.Add(message);
                return currentConversation;
            }
        }
    }
}
