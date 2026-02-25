namespace DotNetSnmp.Protocol.V3.Security
{
    [Flags]
    public enum MsgFlags : byte
    {
        NoAuthNoPriv = 0,
        Auth = 1,
        Priv = 2,
        Reportable = 4,
    }
}
