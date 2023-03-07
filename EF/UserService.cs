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

        internal static Message CreateEmoji(Message message, Reactor reactor)
        {
            message.Reactors.Add(reactor);
            reactor.Emoji.Quantity++;
            message.Emojies.Add(reactor.Emoji);
            return message;
        }

        internal static Message RemoveEmoji(Message message, User user)
        {
            //check which emoji is create by user
            //Emoji? result = message.Reactors.FirstOrDefault(e => message.Emojies.Contains(e.Emoji));
            //Emoji result = message.Emojies.FirstOrDefault(e => e.Equals(message.Reactors.FirstOrDefault(r => r.Emoji == e)));

            Reactor reactor = message.Reactors.FirstOrDefault(r => r.Id.Equals(user.Id));
            if (reactor != null)
            {
                message.Emojies.Remove(reactor.Emoji);
                message.Reactors.Remove(reactor);
                return message;
            }
            else
            {
                return message;
            }

        }

        internal static Message Remove(Conversation conversation, Message message, User sender)
        {
            Message? mes = GetMessageInConversation(conversation, message);
            bool isOwner = CheckMessageOwner(mes, sender);

            if (mes is null && !isOwner)
            {
                return message;
            }
            else
            {
                mes.IsDeleted = true;
                return mes;
            }
        }

        internal static Message Undo(Conversation conversation, Message message, User sender)
        {
            Message? mes = GetMessageInConversation(conversation, message);
            bool isOwner = CheckMessageOwner(mes, sender);

            if (mes is null && !isOwner)
            {
                return message;
            }
            else
            {
                mes.IsDeleted = false;
                return mes;
            }

        }

        internal static Message ReplyMessage(Conversation conversation, Message newMes, Message oldMes)
        {
            if (conversation.Messages.Contains(oldMes) && newMes.Reply is null)
            {
                newMes.Reply = oldMes;
                return newMes;
            }
            else
            {
                return newMes;
            }
        }

        internal static Message EditContent(Conversation conversation, User sender, Message message, string newContent)
        {
            Message? mes = GetMessageInConversation(conversation, message);
            bool isOwner = CheckMessageOwner(mes, sender);

            if (mes is null && !isOwner)
            {
                return message;
            }
            else
            {
                mes.Content = newContent;
                return mes;
            }
        }

        internal static void ReplaceEmoji(User? sender, Message message, Emoji emoji2)
        {
            if (message != null)
            {
                Emoji? emoji = message.Emojies.Find(e => e.Sender == sender);
                if (emoji != null)
                {
                    message.Emojies.Remove(emoji);
                    message.Emojies.Add(emoji2);
                }
            }
        }

        private static bool CheckMessageOwner(Message? message, User sender)
        {
            //check if owner of the message is sender
            if (message != null)
            {
                return message.Sender.Id.Equals(sender.Id);
            }
            return false;
        }

        private static Message? GetMessageInConversation(Conversation conversation, Message message)
        {
            //check if message in that conversation
            //and user is owner
            return conversation.Messages.Find(m => m.Id.Equals(message.Id));
        }
    }
}
