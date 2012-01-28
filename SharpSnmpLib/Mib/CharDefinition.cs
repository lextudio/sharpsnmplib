namespace Lextm.SharpSnmpLib.Mib
{
    public class CharDefinition
    {
        public ISmiValue Name { get; set; }

        public CharDefinition(ISmiValue name)
        {
            Name = name;
        }

        public CharDefinition(string name)
        {

        }
    }
}