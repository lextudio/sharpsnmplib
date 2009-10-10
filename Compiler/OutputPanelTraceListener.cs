using System.Diagnostics;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib.Compiler
{
    public class OutputPanelTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            // leave this empty so event level information is not logged in the OutputPanel.
        }

        public override void WriteLine(string message)
        {
            IOutputPanel content = Program.Container.Resolve<DockContent>("Output") as IOutputPanel;
            if (content != null)
            {
                content.WriteLine(message);
            }
        }
    }
}
