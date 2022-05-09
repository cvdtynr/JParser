using System;

namespace JParser
{
    internal class JSONParseException
        : Exception
    {
        public JSONParseException(string message)
            : base(message)
        {
        }
    }
}
