using Infrastructure;

namespace Messages
{
    public class StringMessage : IMessage
    {
        private readonly string _s;
        public string S => _s;

        public static StringMessage Create(string s)
        {
            return new StringMessage(s);
        }

        private StringMessage(string s)
        {
            _s = s;
        }
    }
}