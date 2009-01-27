using System.Diagnostics;

namespace Lextm.SharpSnmpLib.Browser
{
    public class OutputPanelTraceListener: TraceListener
    {
        public override void Write(string message)
        {
            // leave this empty so event level information is not logged in the OutputPanel.
        }

        public override void WriteLine(string message)
        {
           Program.OutputPanel.WriteLine(message);
        }
    }
}
