namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class Macro : IEntity
    {
        public string Name { get; set; }

        public Macro(string name)
        {
            Name = name;
        }
    }
}