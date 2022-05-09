using System;
using System.Runtime.Serialization;

namespace JParser
{
    [Serializable]
    internal class JSONTypeException : Exception
    {
        public JSONTypeException()
        {
        }

        public JSONTypeException(string message) : base(message)
        {
        }

        public JSONTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JSONTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}