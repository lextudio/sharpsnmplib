namespace Lextm.SharpSnmpLib.Mib
{
    public class NormalConstraintElement : ConstraintElement
    {
        public NormalConstraintElement(ConstraintElement constraintElement)
        {
            Element = constraintElement;
        }

        public NormalConstraintElement()
        {

        }
    }
}