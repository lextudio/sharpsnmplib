namespace Lextm.SharpSnmpLib.Mib
{
    public class ConstraintElement
    {
        public void Add(ConstraintElement element)
        {
            Element = element;
        }

        public ConstraintElement Element;
    }
}