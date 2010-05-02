using Lextm.SharpSnmpLib.Compiler;
using log4net.Appender;
using log4net.Core;
using Microsoft.Practices.Unity;
using WeifenLuo.WinFormsUI.Docking;

namespace Lextm.SharpSnmpLib
{
    public class OutputPanelAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            IOutputPanel content = Program.Container.Resolve<DockContent>("Output") as IOutputPanel;
            if (content != null)
            {
                content.Write(RenderLoggingEvent(loggingEvent));
            }
        }
    }
}
