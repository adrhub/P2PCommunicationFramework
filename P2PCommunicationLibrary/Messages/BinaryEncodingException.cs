using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PCommunicationLibrary.Messages
{
    class BinaryEncodingException : Exception
    {
        public BinaryEncodingException()
            : base()
        {
        }

        public BinaryEncodingException(string message)
            : base(message)
        {
        }

        public BinaryEncodingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
