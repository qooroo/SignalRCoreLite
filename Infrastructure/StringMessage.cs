namespace Infrastructure
{
    public class StringMessage : IMessage
    {
        public string S { get; }

        public static StringMessage Create(string s)
        {
            return new StringMessage(s);
        }

        private StringMessage(string s)
        {
            S = s;
        }
    }
}