namespace Lextm.SharpSnmpLib.Mib
{
    public class ValueRange {
        public bool MaxValue;
        public bool GreaterThan;
        public ISmiValue LowerValue;
        public bool MinValue;
        public bool LessThan;
        public ISmiValue UpperValue;
    }
}