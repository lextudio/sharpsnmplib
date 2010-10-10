using System.Net;

namespace Lextm.SharpSnmpLib.Messaging
{
    internal sealed class MessageParams
    {
        private readonly byte[] _bytes;

        public MessageParams(byte[] bytes, int number, EndPoint sender)
        {
            _bytes = bytes;
            Number = number;
            Sender = (IPEndPoint)sender;
        }

        public byte[] GetBytes()
        {
           return _bytes;
        }

        public IPEndPoint Sender { get; private set; }

        public int Number { get; private set; }
    }
}