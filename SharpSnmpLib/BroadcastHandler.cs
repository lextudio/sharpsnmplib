/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/14
 * Time: 10:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Description of BroadcastHandler.
    /// </summary>
    internal sealed class BroadcastHandler
    {
        private IPEndPoint _endpoint;
        private int _timeout;
        private IDictionary<IPEndPoint, Variable> list = new Dictionary<IPEndPoint, Variable>();
        
        public BroadcastHandler(int timeout, IPEndPoint endpoint)
        {
            _timeout = timeout;
            _endpoint = endpoint;
        }
        
        public IDictionary<IPEndPoint, Variable> Found
        {
            get
            {
                using (TrapListener listener = new TrapListener())
                {
                    listener.GetResponseReceived += delegate(object sender, GetResponseReceivedEventArgs e) { list.Add(e.Sender, e.GetResponse.Variables[0]); };
                    listener.Start(_endpoint);
                    Thread.Sleep(_timeout);
                    listener.Stop();
                    while (listener.Active)
                    {
                        Thread.Sleep(100);
                    }
                }
                return list;
            }
        }        
    }
}
