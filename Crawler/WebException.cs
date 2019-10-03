using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler
{
    class WebException : Exception
    {

        private string message;

        public WebException(string message) : base(message)
        {
            this.message = message;
        }

        public string getMessage()
        {
            return this.message;
        }
    }
}
