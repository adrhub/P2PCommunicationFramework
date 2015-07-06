using System;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary
{
    internal delegate void MessageReceivedEventHandler(IClient sender, MessageEventArgs messageArgs);

    class MessageEventArgs : EventArgs
    {
        public BinaryMessageBase Message { get; private set; }

        public MessageEventArgs(BinaryMessageBase message)
        {
            Message = message;
        }   
    }
}
