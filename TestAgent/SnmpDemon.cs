/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/28/2009
 * Time: 12:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 using System;
using System.Windows.Forms;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// SNMP demon class, who works as a basic agent.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SnmpDemon : IDisposable
    {
        private readonly Listener _listener;
        private readonly SnmpApplicationFactory _factory;
        private readonly AgentObjects _objects;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpDemon"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="listener">The listener.</param>
        /// <param name="objects">Agent core objects.</param>
        public SnmpDemon(SnmpApplicationFactory factory, Listener listener, AgentObjects objects)
        {
            _factory = factory;
            _listener = listener;
            _objects = objects;
        }

        private void ListenerMessageReceived(object sender, MessageReceivedEventArgs<ISnmpMessage> e)
        {
            ISnmpMessage request = e.Message;
            SnmpContext context = SnmpContextFactory.Create(request, e.Sender, _listener, _objects);   
            SnmpApplication application = _factory.Create(context);
            application.Process();
        }

        /// <summary>
        /// Starts the specified port.
        /// </summary>
        /// <param name="port">The port.</param>
        public void Start(int port)
        {
            _listener.ExceptionRaised += ListenerExceptionRaised;
            _listener.MessageReceived += ListenerMessageReceived;
            _listener.Start(port);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            _listener.Stop();
            _listener.ExceptionRaised -= ListenerExceptionRaised;
            _listener.MessageReceived -= ListenerMessageReceived;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SnmpDemon"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active
        {
            get { return _listener.Active; }
        }
     
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private static void ListenerExceptionRaised(object sender, ExceptionRaisedEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _listener.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
