using DotNetSnmp.Common.Definitions;

namespace DotNetSnmp.Common
{
    public class SnmpRequestException : Exception
    {
        public SnmpRequestException(ErrorCode err, int errIdx)
            : base($"The Agent responded with error {err}({(byte)err}) at index {errIdx}")
        {

        }
    }
}
