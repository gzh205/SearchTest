using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler
{
    public class WebException : Exception
    {

        public string message { protected set; get; }

        public WebException(string message) : base(message)
        {
            this.message = message;
        }

        public WebException(string message, Exception innerException) : base(message, innerException)
        {
            this.message = message;
        }

        public WebException()
        {
        }

        protected WebException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
