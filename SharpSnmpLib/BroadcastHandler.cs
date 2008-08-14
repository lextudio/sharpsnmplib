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
    internal class BroadcastHandler
    {
        private TrapListener listener;
        private IPEndPoint _endpoint;
        private int _timeout;
        private IDictionary<IPEndPoint, ISnmpData> list = new Dictionary<IPEndPoint, ISnmpData>();
        
        public BroadcastHandler(int timeout, IPEndPoint endpoint)
        {
            _timeout = timeout;
            _endpoint = endpoint;
            listener = new TrapListener();
            listener.GetResponseReceived += delegate(object sender, GetResponseReceivedEventArgs e) { list.Add(e.Sender, e.GetResponse.Variables[0].Data); };
        }
        
        public IDictionary<IPEndPoint, ISnmpData> Found
        {
            get 
            {                            
                listener.Start(_endpoint.Address, _endpoint.Port);
                Thread.Sleep(_timeout);
                listener.Stop();
                while (listener.Active)
                {
                    Thread.Sleep(100);
                }                
                return list;
            }
        }
    }
}
