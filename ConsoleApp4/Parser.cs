using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JParser
{
    internal class Parser
    {
        private Lexer lexer;
        private List<JToken> tokens;
        private int Cursor;

        private JToken CurrentJToken => tokens[Cursor];
        private bool HasMore => Cursor < tokens.Count;
        private JToken PeekPrevious => tokens[Cursor - 1];

        public Parser(string text)
        {
            this.Cursor = 0;
            this.lexer = new Lexer(text);

            tokens = new List<JToken>();
            JToken token = null;
            do
            {
                token = lexer.GetToken();
                tokens.Add(token);
            } while (token.TokenType != TokenType.END_DOCUMENT);

            CheckExpectToken(CurrentJToken.TokenType, TokenType.BEGIN_OBJECT);
            Cursor++;
        }

        public JObject ParseJObject()
        {
            JObject jObject = new JObject();
            TokenType[] expectedTokens = new[] { TokenType.STRING, TokenType.END_OBJECT };

            string key = null;

            while (HasMore)
            {
                switch (CurrentJToken.TokenType)
                {
                    case TokenType.BEGIN_OBJECT:
                        CheckExpectToken(CurrentJToken.TokenType, expectedTokens);
                        jObject.Add(key, ParseJObject());
                        expectedTokens = new[] { TokenType.SEP_COMMA, TokenType.END_OBJECT };
                        Cursor++;
                        break;
                    case TokenType.END_OBJECT:
                        CheckExpectToken(CurrentJToken.TokenType, expectedTokens);
                        return jObject;
                    //case TokenType.BEGIN_ARRAY:
                    //    CheckExpectToken(CurrentJToken.TokenType, expectedTokens);
                    //    break;
                    //case TokenType.END_ARRAY:
                    //    CheckExpectToken(CurrentJToken.TokenType, expectedTokens);
                    //    break;
                    case TokenType.NULL:
                        CheckExpectToken(CurrentJToken.TokenType, expectedTokens);
                        jObject.Add(key, null);
                        expectedTokens = new[] { TokenType.SEP_COMMA, TokenType.END_OBJECT };
                        Cursor++;
                        break;
                    case TokenType.NUMBER:
                        CheckExpectToken(CurrentJToken.TokenType, expectedTokens);
                        jObject.Add(key, Convert.ToInt32(CurrentJToken.Value));
                        expectedTokens = new[] { TokenType.SEP_COMMA, TokenType.END_OBJECT };
                        Cursor++;
                        break;
                    case TokenType.STRING:
                        CheckExpectToken(CurrentJToken.TokenType, expectedTokens);
                        JToken preToken = PeekPrevious;
                        if (preToken.TokenType == TokenType.SEP_COLON)
                        {
                            jObject.Add(key, CurrentJToken.Value);
                            expectedTokens = new[] { TokenType.SEP_COMMA, TokenType.END_OBJECT };
                        }
                        else
                        {
                            key = CurrentJToken.Value;
                            expectedTokens = new[] { TokenType.SEP_COLON };
                        }
                        Cursor++;
                        break;
                    case TokenType.BOOLEAN:
                        CheckExpectToken(CurrentJToken.TokenType, expectedTokens);
                        jObject.Add(key, Convert.ToBoolean(CurrentJToken.Value));
                        expectedTokens = new[] { TokenType.SEP_COMMA, TokenType.END_OBJECT };
                        Cursor++;
                        break;
                    case TokenType.SEP_COLON:
                        CheckExpectToken(CurrentJToken.TokenType, expectedTokens);
                        expectedTokens = new[] {
                            TokenType.NULL,
                            TokenType.NUMBER,
                            TokenType.BOOLEAN,
                            TokenType.STRING,
                            TokenType.BEGIN_OBJECT,
                            TokenType.BEGIN_ARRAY,
                        };
                        Cursor++;
                        break;
                    case TokenType.SEP_COMMA:
                        CheckExpectToken(CurrentJToken.TokenType, expectedTokens);
                        expectedTokens = new[] { TokenType.STRING };
                        Cursor++;
                        break;
                    case TokenType.END_DOCUMENT:
                        CheckExpectToken(CurrentJToken.TokenType, expectedTokens);
                        return jObject;
                    default:
                        throw new JSONParseException("Unexpected Token.");
                }

            }

            throw new JSONParseException("Parse error, invalid Token.");

        }

        private void CheckExpectToken(TokenType tokenType, params TokenType[] expectTokens)
        {
            if (!expectTokens.Contains(tokenType))
                throw new JSONParseException("Parse error, invalid Token.");
        }
    }
}
