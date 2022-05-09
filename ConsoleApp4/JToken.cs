namespace JParser
{
    public enum TokenType
    {
        BEGIN_OBJECT,
        END_OBJECT,
        BEGIN_ARRAY,
        END_ARRAY,
        NULL,
        NUMBER,
        STRING,
        BOOLEAN,
        SEP_COLON,
        SEP_COMMA,
        END_DOCUMENT
    }

    public class JToken
    {
        public string Value { get; }
        public int Position { get; }
        public TokenType TokenType { get; }

        public JToken(TokenType tokenType, string value, int position)
        {
            Value = value;
            Position = position;
            TokenType = tokenType;
        }

        public override string ToString()
        {
            return $"TokenType = {TokenType}, Value = '{Value}', Position = {Position}";
        }
    }
}
