/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/28/2009
 * Time: 12:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
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
        private SecurityGuard _guard = new SecurityGuard(VersionCode.V1, new OctetString("public"), new OctetString("public"));
        private ObjectStore _store = new ObjectStore();
        private Logger _logger = new Logger();
        private Listener _listener = new Listener();
        
        public SnmpDemon()
        {
            _listener.ExceptionRaised += Agent1ExceptionRaised;
            DefaultListenerAdapter adapter = new DefaultListenerAdapter(_listener);
            _listener.Adapters.Add(adapter);
            adapter.GetRequestReceived += Agent1GetRequestReceived;
            adapter.GetNextRequestReceived += AdapterGetNextRequestReceived;
            adapter.GetBulkRequestReceived += AdapterGetBulkRequestReceived;
            adapter.SetRequestReceived += AdapterSetRequestReceived;
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
        
        private void AdapterSetRequestReceived(object sender, MessageReceivedEventArgs<SetRequestMessage> e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            SetRequestMessage message = e.Message;

            if (!_guard.Allow(message))
            {
                // TODO: return error message.
                return;
            }

            IList<Variable> result = new List<Variable>();
            foreach (Variable v in message.Variables)
            {
                ISnmpObject obj = _store.GetObject(v.Id);
                obj.Set(v.Data);
                result.Add(v);
            }

            GetResponseMessage response = new GetResponseMessage(message.RequestId, message.Version, message.Community, ErrorCode.NoError, 0, result);
            _listener.SendResponse(response, e.Sender);
            _logger.Log(_listener.Port, message.Pdu.TypeCode, response, e.Sender, watch.ElapsedMilliseconds);
            watch.Stop();
        }

        private void AdapterGetBulkRequestReceived(object sender, MessageReceivedEventArgs<GetBulkRequestMessage> e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            GetBulkRequestMessage message = e.Message;
            // you may validate message version number and/or community name here.
            if (message.Variables.Count != 1)
            {
                return;
            }

//            if (message.Variables[0].Id != SysDescr)
//            {
//                return;
//            }

            List<Variable> list = new List<Variable>();
            //list.Add(new Variable(SysDescr, new EndOfMibView()));

            GetResponseMessage response = new GetResponseMessage(message.RequestId, message.Version, message.Community, ErrorCode.NoError, 0, list);
            _listener.SendResponse(response, e.Sender);
            _logger.Log(_listener.Port, message.Pdu.TypeCode, response, e.Sender, watch.ElapsedMilliseconds);
            watch.Stop();
        }

        private void AdapterGetNextRequestReceived(object sender, MessageReceivedEventArgs<GetNextRequestMessage> e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            GetNextRequestMessage message = e.Message;

            if (!_guard.Allow(message))
            {
                // TODO: return error message.
                return;
            }

            bool hasError = false;
            int index = 0;
            IList<Variable> result = new List<Variable>();
            foreach (Variable v in message.Variables)
            {
                index++;
                ISnmpObject next = _store.GetNextObject(v.Id);
                if (next == null)
                {
                    hasError = true;
                    result.Add(v);
                    break;
                }

                result.Add(new Variable(next.Id, next.Get()));
            }

            GetResponseMessage response;
            if (hasError)
            {
                response = new GetResponseMessage(message.RequestId, message.Version, message.Community, ErrorCode.NoSuchName, index, result);
            }
            else
            {
                response = new GetResponseMessage(message.RequestId, message.Version, message.Community, ErrorCode.NoError, 0, result);
            }

            _listener.SendResponse(response, e.Sender);
            _logger.Log(_listener.Port, message.Pdu.TypeCode, response, e.Sender, watch.ElapsedMilliseconds);
            watch.Stop();
        }

        private void Agent1GetRequestReceived(object sender, MessageReceivedEventArgs<GetRequestMessage> e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            GetRequestMessage message = e.Message;

            if (!_guard.Allow(message))
            {
                // TODO: return error message.
                return;
            }

            bool hasError = false;
            int index = 0;
            IList<Variable> result = new List<Variable>();
            foreach (Variable v in message.Variables)
            {
                index++;
                ISnmpObject obj = _store.GetObject(v.Id);
                if (obj != null)
                {
                    result.Add(new Variable(v.Id, obj.Get()));
                }
                else
                {
                    hasError = true;
                    result.Add(v);
                    break;
                }
            }

            GetResponseMessage response;
            if (hasError)
            {
                response = new GetResponseMessage(message.RequestId, message.Version, message.Community, ErrorCode.NoSuchName, index, result);
            }
            else
            {
                response = new GetResponseMessage(message.RequestId, message.Version, message.Community, ErrorCode.NoError, 0, result);
            }

            _listener.SendResponse(response, e.Sender);
            _logger.Log(_listener.Port, message.Pdu.TypeCode, response, e.Sender, watch.ElapsedMilliseconds);
            watch.Stop();
        }
        
        private static void Agent1ExceptionRaised(object sender, ExceptionRaisedEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }
    }
}
