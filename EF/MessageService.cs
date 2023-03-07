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
                    //Sender = x.Sender,

                })
                .DistinctBy(x => x.Unicode)
                .OrderBy(x => x.CreatedAt)
                .ThenByDescending(x => x.Quantity)
                .ToList();
            //.GroupBy(i => i.Unicode)
            //.OrderByDescending(i => i.Count())
            //.ThenBy(o=>o.CreatedAt)
            //.Select(g => new { Unicode = g.Key, Quantity = g.Count() });

            return message;
        }

        internal static Message CreateEmoji(Message message, Emoji emoji)
        {
            emoji.Quantity++;
            message.Emojies.Add(emoji);
            return message;
        }

        internal static Message RemoveEmoji(Message message, User user)
        {
            //check which emoji is create by user
            Emoji? result = message.Emojies.FirstOrDefault(m => m.Sender.Id.Equals(user.Id));
            if (result != null)
            {
                message.Emojies.Remove(result);
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
    }
}
