using System.Diagnostics;

namespace Lextm.SharpSnmpLib.Browser
{
    public class OutputPanelTraceListener: TraceListener
    {
        private static OutputPanel _panel;

        internal static OutputPanel Panel
        {
            set { _panel = value; }
        }

        public override void Write(string message)
        {
            // leave this empty so event level information is not logged in the OutputPanel.
        }

        public override void WriteLine(string message)
        {
            if (_panel == null)
            {
                return;
            }

            _panel.WriteLine(message);
        }
    }
}
