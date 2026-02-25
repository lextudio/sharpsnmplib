using DotNetSnmp.Common.Definitions;

namespace DotNetSnmp.Common
{
    /// <summary>
    /// Represents the SnmpRequestException type.
    /// </summary>
    public class SnmpRequestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of SnmpRequestException.
        /// </summary>
        public SnmpRequestException(ErrorCode err, int errIdx)
            : base($"The Agent responded with error {err}({(byte)err}) at index {errIdx}")
        {

        }
    }
}
