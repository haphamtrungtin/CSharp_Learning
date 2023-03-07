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
                //Emojies = new List<Emoji>(),
                Reactors = new HashSet<Reactor>()
            };
            Emoji emoji = EmojiService.GetEmojies().FirstOrDefault();
            Reactor reactor = new Reactor()
            {
                Id = 1,
                Emoji = emoji,
                User = user
            };

            Message result = UserService.CreateEmoji(message, reactor);

            Assert.IsTrue(result.Emojies.Count > 0);
        }

        [TestMethod]
        public void RemoveEmojiTest()
        {
            User user = UserService.GetUsers().FirstOrDefault();
            Emoji emoji = EmojiService.GetEmojies().FirstOrDefault();
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 1,
                Sender = user,
                IsEdited = false
            };
            Reactor reactor = new Reactor()
            {
                Id = 1,
                Emoji = emoji,
                User = user
            };

            Message newMes = UserService.CreateEmoji(message, reactor);
            Message result = UserService.RemoveEmoji(newMes, user);

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
            Reactor reactor = new Reactor()
            {
                Id = 1,
                Emoji = emoji1,
                User = user1
            };
            Reactor reactor2 = new Reactor()
            {
                Id = 2,
                Emoji = emoji2,
                User = user2
            };
            Reactor reactor3 = new Reactor()
            {
                Id = 3,
                Emoji = emoji2,
                User = user3
            };

            emoji1.Sender = user2;
            emoji1.CreatedAt = new DateTime(2023, 3, 3);
            UserService.CreateEmoji(message, reactor);

            emoji2.Sender = user1;
            emoji2.CreatedAt = new DateTime(2023, 2, 3);
            UserService.CreateEmoji(message, reactor2);

            emoji2.Sender = user3;
            UserService.CreateEmoji(message, reactor3);

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
            Reactor reactor = new Reactor()
            {
                Id = 1,
                Emoji = emoji1,
                User = user1
            };
            Reactor reactor2 = new Reactor()
            {
                Id = 2,
                Emoji = emoji2,
                User = user2
            };
            Reactor reactor3 = new Reactor()
            {
                Id = 3,
                Emoji = emoji3,
                User = user3
            };
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 1,
                Sender = user1,
                IsEdited = false,
                Emojies = new List<Emoji>(),
                Reactors = new HashSet<Reactor> { reactor, reactor2, reactor3 }
            };

            emoji1.Sender = user2;
            emoji1.CreatedAt = new DateTime(2023, 5, 3);
            UserService.CreateEmoji(message, reactor);

            emoji2.Sender = user1;
            emoji2.CreatedAt = new DateTime(2023, 4, 3);
            UserService.CreateEmoji(message, reactor2);

            emoji3.Sender = user3;
            emoji3.CreatedAt = new DateTime(2023, 3, 3);
            UserService.CreateEmoji(message, reactor3);

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

            Message deletedMessage = UserService.Remove(conversation, message, sender);

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

            Message undoMessage = UserService.Undo(conversation, message, sender);

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
            Message reply = UserService.ReplyMessage(conversation, newMes, oldMes);

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
            Message newMes = UserService.EditContent(conversation, sender, message , newContent);

            Assert.AreEqual(message.Content, newMes.Content, newContent);
        }

        [TestMethod]
        public void ReplaceExistingEmojiTest()
        {
            //Arrange
            User sender = UserService.GetUsers().FirstOrDefault();
            Emoji emoji1 = EmojiService.GetEmojies().FirstOrDefault();
            Emoji emoji2 = EmojiService.GetEmojies().LastOrDefault();
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 1,
                Sender = sender,
                IsEdited = false,
                Emojies = new List<Emoji>() { emoji1 }
            };
            emoji1.Sender = sender;

            Assert.IsTrue(message.Emojies.Contains(emoji1));

            UserService.ReplaceEmoji(sender, message, emoji2);

            Assert.IsTrue(message.Emojies.Any(e=>e.Name.Equals(emoji2.Name)));
            Assert.IsTrue(message.Emojies.Any(e=>e.Unicode.Equals(emoji2.Unicode)));
            Assert.IsFalse(message.Emojies.Contains(emoji1));
        }

        [TestMethod]
        public void ShowListReactorTest() 
        {
            //Arrange
            User sender = UserService.GetUsers().FirstOrDefault();
            User sender1 = UserService.GetUsers().LastOrDefault();
            Emoji emoji1 = EmojiService.GetEmojies().FirstOrDefault();
            Emoji emoji2 = EmojiService.GetEmojies().LastOrDefault();
            Reactor reactor = new()
            {
                Id = 1,
                Emoji = emoji1,
                User = sender
            };
            Reactor reactor2 = new()
            {
                Id = 2,
                Emoji = emoji2,
                User = sender1
            };
            Message message = new()
            {
                Content = "Lorem ipsum",
                Date = DateTime.Now,
                Id = 1,
                Sender = sender,
                IsEdited = false,
                Emojies = new List<Emoji>() {},
                Reactors = new HashSet<Reactor> { reactor, reactor2 }
            };

            HashSet<Reactor> reactorList = MessageService.ShowListReactor(message);

            Assert.IsTrue(reactorList.Any(e => e.User == sender));
            Assert.IsTrue(reactorList.Any(e=>e.Emoji.Equals(emoji1)));
            Assert.IsTrue(reactorList.Any(e=>e.Emoji.Equals(emoji2)));
        }
    }
}