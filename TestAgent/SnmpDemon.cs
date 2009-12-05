/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/28/2009
 * Time: 12:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Windows.Forms;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Agent
{
    /// <summary>
    /// Description of SnmpDemon.
    /// </summary>
    public class SnmpDemon
    {
        private readonly Listener _listener = new Listener();
        private readonly SnmpApplicationFactory _factory = new SnmpApplicationFactory(new Logger(), new ObjectStore(),
                                                                                      new Version1MembershipProvider(
                                                                                          VersionCode.V1,
                                                                                          new OctetString("public"),
                                                                                          new OctetString("public")));

        public SnmpDemon()
        {
            _listener.ExceptionRaised += ListenerExceptionRaised;
            _listener.MessageReceived += ListenerMessageReceived;
        }

        private void ListenerMessageReceived(object sender, MessageReceivedEventArgs<ISnmpMessage> e)
        {
            ISnmpMessage request = e.Message;
            SnmpContext context = new SnmpContext(request, null, e.Sender, _listener);   
            SnmpApplication application = _factory.Create(context);
            application.Process();
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
