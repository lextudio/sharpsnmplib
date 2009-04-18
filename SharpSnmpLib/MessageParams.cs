using System.Net;

namespace Lextm.SharpSnmpLib
{
    internal sealed class MessageParams
    {
        private readonly int _number;
        private readonly byte[] _bytes;
        private readonly IPEndPoint _sender;

        public MessageParams(byte[] bytes, int number, EndPoint sender)
        {
            _bytes = bytes;
            _number = number;
            _sender = (IPEndPoint)sender;
        }

        public byte[] GetBytes()
        {
           return _bytes;
        }

        public IPEndPoint Sender
        {
            get { return _sender; }
        }

        public int Number
        {
            get { return _number; }
        }
    }
}