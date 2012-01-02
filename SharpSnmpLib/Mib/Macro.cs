namespace Lextm.SharpSnmpLib.Mib
{
    public class Macro : IConstruct
    {
        public string Name { get; set; }

        public Macro(string name)
        {
            Name = name;
        }
    }
}