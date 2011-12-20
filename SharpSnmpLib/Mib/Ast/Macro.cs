namespace Lextm.SharpSnmpLib.Mib.Ast
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