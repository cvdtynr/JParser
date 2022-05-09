namespace JParser
{
    public class Lexer
    {
        private int _cursor;
        private readonly string _text;

        public Lexer(string text)
        {
            _text = text;
            _cursor = 0;
        }

        private char Current => _cursor >= _text.Length ? '\0' : _text[_cursor];
        private char Peak(int offset) => _cursor + offset >= _text.Length ? '\0' : _text[_cursor + offset];

        private void AcceptChar(char c)
        {
            if (c == Current)
                _cursor++;
            else
                throw new JSONParseException($"Unexpected token '{Current}' at {_cursor}");
        }

        private void SkipWhitespace()
        {
            while (char.IsWhiteSpace(Current)) _cursor++;
        }

        public JToken GetToken()
        {
            if (Current == '\0')
                return new JToken(TokenType.END_DOCUMENT, Current.ToString(), _cursor);

            SkipWhitespace();

            switch (Current)
            {
                case '{':
                    return new JToken(TokenType.BEGIN_OBJECT, Current.ToString(), _cursor++);
                case '}':
                    return new JToken(TokenType.END_OBJECT, Current.ToString(), _cursor++);
                case '[':
                    return new JToken(TokenType.BEGIN_ARRAY, Current.ToString(), _cursor++);
                case ']':
                    return new JToken(TokenType.END_ARRAY, Current.ToString(), _cursor++);
                case ',':
                    return new JToken(TokenType.SEP_COMMA, Current.ToString(), _cursor++);
                case ':':
                    return new JToken(TokenType.SEP_COLON, Current.ToString(), _cursor++);
                case 't':
                case 'f':
                    return CreateBooleanToken();
                case 'n':
                    return CreateNullToken();
                case '"':
                    return CreateStringToken();
                case '-':
                    return CreateNumberToken();
                default:
                    break;
            }

            if (char.IsDigit(Current))
                return CreateNumberToken();

            throw new JSONParseException($"Unexpected token '{Current}' at {_cursor}");
        }

        private JToken CreateNumberToken()
        {
            int start = _cursor;

            if (Current == '-')
                _cursor++;

            while (char.IsNumber(Current))
                _cursor++;

            return new JToken(TokenType.NUMBER, _text.Substring(start, _cursor - start), start);
        }

        private JToken CreateStringToken()
        {
            // TODO : \"

            AcceptChar('"');

            int start = _cursor;

            while (Current != '"')
                _cursor++;

            AcceptChar('"');

            return new JToken(TokenType.STRING, _text.Substring(start, (_cursor - 1) - start), start);
        }

        private JToken CreateNullToken()
        {
            int start = _cursor;
            var nullStr = "null";

            foreach (var c in nullStr)
                AcceptChar(c);

            return new JToken(TokenType.NULL, nullStr, start);
        }

        private JToken CreateBooleanToken()
        {
            int start = _cursor;
            var str = Current == 'f' ? "false" : "true";

            foreach (var c in str)
                AcceptChar(c);

            SkipWhitespace();

            return new JToken(TokenType.BOOLEAN, str, start);
        }
    }
}
