namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public interface IEntity
    {
        long Value { get; set; }
        string Parent { get; set; }
        string Name { get; set; }
    }
}