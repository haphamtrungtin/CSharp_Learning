namespace EF
{
    internal class EmojiService
    {
        private static List<Emoji> _emojies = new List<Emoji>(){
                        new Emoji(){Id=1, Name="Grinning Face", Category="Emoticons",  IsDefault=true ,Shortcut=":D" , Unicode="U+1F600" },
                        new Emoji(){Id=2, Name="Grinning Face with Smiling Eyes", Category="Emoticons",  IsDefault=true ,Shortcut=":)", Unicode="U+1F601" },
                        new Emoji(){Id=3, Name="Face with Tears of Joy", Category="Emoticons",  IsDefault=true ,Shortcut=":))", Unicode="U+1F602" },
                        new Emoji(){Id=4, Name="Smiling Face with Open Mouth", Category="Emoticons",  IsDefault=false ,Shortcut=":X", Unicode="U+1F603"},
                        new Emoji(){Id=5, Name="Smiling Face with Open Mouth and Smiling Eyes", Category="Emoticons",  IsDefault=false ,Shortcut=":V", Unicode="U+1F604" },
                    };

        internal static List<Emoji> GetEmojies()
        {
            return _emojies;
        }
    }
}