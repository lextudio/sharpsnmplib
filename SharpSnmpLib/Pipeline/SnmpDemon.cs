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

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// SNMP demon class, who works as a basic agent.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    public sealed class SnmpDemon : IDisposable
    {
        private readonly SnmpApplicationFactory _factory;
        private readonly DemonObjects _objects;  

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpDemon"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="listener">The listener.</param>
        /// <param name="objects">Agent core objects.</param>
        public SnmpDemon(SnmpApplicationFactory factory, Listener listener, DemonObjects objects)
        {
            _factory = factory;
            Listener = listener;
            _objects = objects;
        }        
        
        /// <summary>
        /// Gets or sets the listener.
        /// </summary>
        /// <value>The listener.</value>
        public Listener Listener { get; private set; }

        private void ListenerMessageReceived(object sender, MessageReceivedEventArgs<ISnmpMessage> e)
        {
            ISnmpMessage request = e.Message;
            SnmpContext context = SnmpContextFactory.Create(request, e.Sender, Listener.Users, _objects, e.Binding);   
            SnmpApplication application = _factory.Create(context);
            application.Process();
        }

        /// <summary>
        /// Starts the demon.
        /// </summary>
        public void Start()
        {   
            Listener.ExceptionRaised += ListenerExceptionRaised;
            Listener.MessageReceived += ListenerMessageReceived;
            Listener.Start();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            Listener.Stop();
            Listener.ExceptionRaised -= ListenerExceptionRaised;
            Listener.MessageReceived -= ListenerMessageReceived;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SnmpDemon"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active
        {
            get { return Listener.Active; }
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
            Listener.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
