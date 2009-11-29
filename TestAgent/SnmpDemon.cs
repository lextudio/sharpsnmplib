/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/28/2009
 * Time: 12:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Description of SnmpDemon.
    /// </summary>
    public class SnmpDemon
    {
        private readonly SecurityGuard _guard = new SecurityGuard(VersionCode.V1, new OctetString("public"), new OctetString("public"));
        private readonly ObjectStore _store = new ObjectStore();
        private readonly Logger _logger = new Logger();
        private readonly Listener _listener = new Listener();
        private readonly MessageHandlerFactory _factory;
        private const int MaxResponseSize = 1500;

        public SnmpDemon()
        {
            _factory = new MessageHandlerFactory(_store);
            _listener.ExceptionRaised += ListenerExceptionRaised;
            _listener.MessageReceived += ListenerMessageReceived;
        }

        private void ListenerMessageReceived(object sender, MessageReceivedEventArgs<ISnmpMessage> e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            ISnmpMessage message = e.Message;

            if (!_guard.Allow(message))
            {
                // TODO: handle error here.
                watch.Stop();
                return;
            }

            IMessageHandler handler = _factory.GetHandler(message);
            if (handler == null)
            {
                // TODO: handle error here.
                watch.Stop();
                return;
            }

            IList<Variable> result = handler.Handle();
            
            GetResponseMessage response;
            if (handler.ErrorStatus == ErrorCode.NoError)
            {
                response = new GetResponseMessage(message.RequestId, message.Version, message.Parameters.UserName,
                                                  ErrorCode.NoError, 0, result);
                if (response.ToBytes().Length > MaxResponseSize)
                {
                    response = new GetResponseMessage(message.RequestId, message.Version, message.Parameters.UserName,
                                                      ErrorCode.TooBig, 0, message.Pdu.Variables);
                }
            }
            else
            {
                response = new GetResponseMessage(message.RequestId, message.Version, message.Parameters.UserName,
                                                  handler.ErrorStatus, handler.ErrorIndex, message.Pdu.Variables);
            }

            _listener.SendResponse(response, e.Sender);
            _logger.Log(_listener.Port, message.Pdu.TypeCode, response, e.Sender, watch.ElapsedMilliseconds);
            watch.Stop();
        }
        
        public void Start(int port)
        {
            _listener.Start(port);
        }
        
        public void Stop()
        {
            _listener.Stop();
        }
        
        public bool Active
        {
            get { return _listener.Active; }
        }
     
        private static void ListenerExceptionRaised(object sender, ExceptionRaisedEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }
    }
}
