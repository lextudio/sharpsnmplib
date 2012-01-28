namespace Lextm.SharpSnmpLib.Mib
{
    public class ChoiceValue : ISmiValue
    {
        public string Name { get; set; }

        public bool ContainsColon { get; set; }

        public ISmiValue Value { get; set; }
    }
}