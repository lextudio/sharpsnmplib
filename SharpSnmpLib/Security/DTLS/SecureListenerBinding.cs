using DTLS;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharpSnmpLib.DTLS
{
    public class SecureListenerBinding
    {
        private Server _server;
        private readonly UserRegistry _users;

        public IPEndPoint Endpoint { get; }

        public SecureListenerBinding(UserRegistry users, IPEndPoint endpoint)
        {
            _server = new Server(endpoint);
            _server.DataReceived += new Server.DataReceivedEventHandler(server_DataReceived);
            _server.PSKIdentities.AddIdentity(Encoding.UTF8.GetBytes("oFIrQFrW8EWcZ5u7eGfrkw"), ByteTool.Convert("7CCDE14A5CF3B71C0C08C8B7F9E5"));
            _server.LoadCertificateFromPem("TestServer.pem");
            _server.RequireClientCertificate = true;
            _users = users;
            Endpoint = endpoint;
        }

        public void Start()
        {
            _server.Start();
        }

        private void server_DataReceived(EndPoint remote, byte[] data)
        {
            IList<ISnmpMessage> messages = null;
            try
            {
                messages = MessageFactory.ParseMessages(data, 0, data.Length, _users);
            }
            catch (Exception ex)
            {
                var exception = new MessageFactoryException("Invalid message bytes found. Use tracing to analyze the bytes.", ex);
                exception.SetBytes(data);
                HandleException(exception);
            }

            if (messages == null)
            {
                return;
            }

            foreach (var message in messages)
            {
                var handler = MessageReceived;
                handler?.Invoke(this, new MessageReceivedEventArgs((IPEndPoint)remote, message, this));
            }
        }

        private void HandleException(Exception exception)
        {
            ExceptionRaised?.Invoke(this, new ExceptionRaisedEventArgs(exception));
        }

        public void SendResponse(ISnmpMessage response, EndPoint receiver)
        {
            _server.Send(receiver, response.ToBytes());
        }

        public Task SendResponseAsync(ISnmpMessage response, EndPoint receiver)
        {
            // TODO: make it true async.
            return Task.Factory.StartNew(() => _server.Send(receiver, response.ToBytes()));
        }

        public void Stop()
        {
            _server.Stop();
        }

        public async Task StartAsync()
        {
            // TODO:
            await Task.Yield();
        }

        public void Dispose()
        {
            // TODO:
        }

        /// <summary>
        /// Occurs when an exception is raised.
        /// </summary>
        /// <remarks>The exception can be both <see cref="SocketException"/> and <see cref="SnmpException"/>.</remarks>
        public event EventHandler<ExceptionRaisedEventArgs> ExceptionRaised;

        /// <summary>
        /// Occurs when a message is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }
}
