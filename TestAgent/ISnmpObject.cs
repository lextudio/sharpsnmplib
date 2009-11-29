namespace Lextm.SharpSnmpLib.Agent
{
    internal interface ISnmpObject
    {
        ISnmpData Data { get; set; }
        ObjectIdentifier Id { get; }
    }
}
