using System.Security.Cryptography;

namespace P2PCommunicationLibrary.Net
{
    class Token
    {
        public static readonly int TOKEN_LENGTH = 4
            ;
        private static readonly RNGCryptoServiceProvider RngCryptoServiceProvider = new RNGCryptoServiceProvider();

        public string Key { get; private set; }

        private Token(string key)
        {
            Key = key;
        }

        private static string GetToken()
        {
            byte[] array = new byte[TOKEN_LENGTH];
            RngCryptoServiceProvider.GetBytes(array);
            string key = System.Text.Encoding.ASCII.GetString(array);

            return key;
        }

        public static Token GenerateNew()
        {           
            return new Token(GetToken());
        }

        protected bool Equals(Token other)
        {            
            return string.Equals(Key, other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Token) obj);
        }

        public override int GetHashCode()
        {
            return (Key != null ? Key.GetHashCode() : 0);
        }
    }
}
