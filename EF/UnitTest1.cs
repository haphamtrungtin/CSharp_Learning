using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EF
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateEmojiTest()
        {
            User user = UserService.GetUsers().FirstOrDefault();
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 1,
                Sender = user,
                IsEdited = false,
                Emojies = new List<Emoji>()
            };
            Emoji emoji = EmojiService.GetEmojies().FirstOrDefault();
            emoji.Sender = user;


            Message result = MessageService.CreateEmoji(message, emoji);

            Assert.IsTrue(message.Emojies.Count > 0);
        }

        [TestMethod]
        public void RemoveEmojiTest()
        {
            List<User> users = UserService.GetUsers();
            Emoji emoji = EmojiService.GetEmojies().FirstOrDefault();
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 1,
                Sender = users[0],
                IsEdited = false,
                Emojies = new List<Emoji>()
            };

            Message newMes = MessageService.CreateEmoji( message, emoji);
            Message result = MessageService.RemoveEmoji(newMes, users[0]);

            Assert.IsTrue(result.Emojies.Count == 0);
        }

        [TestMethod]
        public void SortEmojiByQuantityTest()
        {
            User user1 = UserService.GetUsers().FirstOrDefault();
            User user2 = UserService.GetUsers().Find(i=>i.Id.Equals(1));
            User user3 = UserService.GetUsers().LastOrDefault();
            Emoji emoji1 = EmojiService.GetEmojies().FirstOrDefault();
            Emoji emoji2 = EmojiService.GetEmojies().LastOrDefault();
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 1,
                Sender = user1,
                IsEdited = false,
                Emojies = new List<Emoji>()
            };

            emoji1.Sender = user2;
            emoji1.CreatedAt = new DateTime(2023, 3, 3);
            MessageService.CreateEmoji(message, emoji1);

            emoji2.Sender = user1;
            emoji2.CreatedAt = new DateTime(2023, 2, 3);
            MessageService.CreateEmoji(message, emoji2);

            emoji2.Sender = user3;
            MessageService.CreateEmoji(message, emoji2);

            Assert.IsTrue(message.Emojies.FirstOrDefault().Quantity < message.Emojies.LastOrDefault().Quantity);
            //current message have emoji[0] with 2 reactor, e[1] with 1 reactor
            //time order emoji[0] -> emoji[1] --> emoji[0]
            //quantity order emoji[0] (2) --> emoji[1] (1)
            MessageService.SortEmojiList(message);

            Assert.IsTrue(message.Emojies.FirstOrDefault().Quantity > message.Emojies.LastOrDefault().Quantity);
        }

        [TestMethod]
        public void SortEmojiByCreatedDateTest()
        {
            User user1 = UserService.GetUsers().FirstOrDefault();
            User user2 = UserService.GetUsers().Find(i => i.Id.Equals(1));
            User user3 = UserService.GetUsers().LastOrDefault();
            Emoji emoji1 = EmojiService.GetEmojies().FirstOrDefault();
            Emoji emoji2 = EmojiService.GetEmojies().LastOrDefault();
            Emoji emoji3 = EmojiService.GetEmojies().Find(i => i.Id.Equals(3));
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 1,
                Sender = user1,
                IsEdited = false,
                Emojies = new List<Emoji>()
            };

            emoji1.Sender = user2;
            emoji1.CreatedAt = new DateTime(2023, 5, 3);
            MessageService.CreateEmoji(message, emoji1);

            emoji2.Sender = user1;
            emoji2.CreatedAt = new DateTime(2023, 4, 3);
            MessageService.CreateEmoji(message, emoji2);

            emoji3.Sender = user3;
            emoji3.CreatedAt = new DateTime(2023, 3, 3);
            MessageService.CreateEmoji(message, emoji3);

            //current message have emoji[0] with 2 reactor, e[1] with 1 reactor
            //time order emoji[0] -> emoji[1] --> emoji[0]
            //quantity order emoji[0] (2) --> emoji[1] (1)
            Assert.IsTrue(message.Emojies.FirstOrDefault().CreatedAt > message.Emojies.LastOrDefault().CreatedAt);
            
            MessageService.SortEmojiList(message);

            Assert.IsTrue(message.Emojies.FirstOrDefault().CreatedAt < message.Emojies.LastOrDefault().CreatedAt);
        }

        [TestMethod]
        public void SendMessageToUserTest()
        {
            User sender = UserService.GetUsers()[0];
            User receiver = UserService.GetUsers()[1];
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 1,
                Sender = sender,
                IsEdited = false,
                Emojies = new List<Emoji>()
            };
            Conversation conversation = UserService.SendMessage(sender, message, receiver);

            Assert.IsTrue(conversation.Users[0].Id == sender.Id);
            Assert.IsTrue(conversation.Users[1].Id == receiver.Id);
            Assert.IsTrue(sender.Conversations.Any(c => c.Id.Equals(conversation.Id)));
        }

        [TestMethod]
        public void RemoveMessageTest()
        {
            User sender = UserService.GetUsers()[0];
            User receiver = UserService.GetUsers()[1];
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 1,
                Sender = sender,
                IsEdited = false,
                IsDeleted = false,
                Emojies = new List<Emoji>()
            };

            Conversation conversation = UserService.SendMessage(sender, message, receiver);

            Message deletedMessage = MessageService.Remove(conversation, message, sender);

            Assert.IsTrue(deletedMessage.IsDeleted);
        }

        [TestMethod]
        public void UndoMessageTest() {
            User sender = UserService.GetUsers()[0];
            User receiver = UserService.GetUsers()[1];
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 1,
                Sender = sender,
                IsEdited = false,
                IsDeleted = true,
                Emojies = new List<Emoji>()
            };

            Conversation conversation = UserService.SendMessage(sender, message, receiver);

            Message undoMessage = MessageService.Undo(conversation, message, sender);

            Assert.IsFalse(undoMessage.IsDeleted);
        }

        [TestMethod]
        public void ReplyMessageTest() 
        {
            User sender = UserService.GetUsers()[0];
            User receiver = UserService.GetUsers()[1];
            Message newMes = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 2,
                Sender = sender,
                IsEdited = false,
                IsDeleted = true,
                Emojies = new List<Emoji>()
            };

            Message oldMes = new Message()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now.AddDays(-1),
                Id = 1,
                Sender = sender,
                IsEdited = false,
                IsDeleted = false,
                Emojies = new List<Emoji>()
            };

            Conversation conversation = UserService.SendMessage(sender, oldMes, receiver);
            Message reply = MessageService.ReplyMessage(conversation, newMes, oldMes);

            Assert.IsTrue(reply.Reply is not null);
            Assert.IsTrue(reply.Reply.Date < reply.Date);
        }

        [TestMethod]
        public void EditMessageTest() 
        {
            User sender = UserService.GetUsers()[0];
            User receiver = UserService.GetUsers()[1];
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 2,
                Sender = sender,
                IsEdited = false,
                IsDeleted = false,
                Emojies = new List<Emoji>()
            };
            string newContent = "Test test";
            Conversation conversation = UserService.SendMessage(sender, message, receiver);
            Message newMes = MessageService.EditContent(conversation, sender, message , newContent);

            Assert.AreEqual(message.Content, newMes.Content, newContent);
        }

        [TestMethod]
        public void ReplaceExistingEmojiTest()
        {
            //Arrange
            User sender = UserService.GetUsers().FirstOrDefault();
            Emoji emoji1 = EmojiService.GetEmojies().FirstOrDefault();
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 1,
                Sender = sender,
                IsEdited = false,
                Emojies = new List<Emoji>() { emoji1 }
            };

            //message.ReplaceEmoji(sender);

            //Assert.IsTrue();
        }

        [TestMethod]
        public void ShowListReactor() { }
    }
}