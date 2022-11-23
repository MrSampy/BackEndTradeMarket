using System;
using System.Runtime.Serialization;

namespace Business.Validation
{
    [Serializable()]
    public class MarketException:Exception
    {
        public MarketException() { }
        public MarketException(string message) : base(message) { }
        public MarketException(string message, System.Exception inner) : base(message, inner) { }
        protected MarketException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}