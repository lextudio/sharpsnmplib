namespace Lextm.SharpSnmpLib.Mib
{
    public class UnknownType : ISmiType
    {
        public UnknownType(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}