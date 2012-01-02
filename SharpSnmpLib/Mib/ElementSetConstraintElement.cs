namespace Lextm.SharpSnmpLib.Mib
{
    public class ElementSetConstraintElement : ConstraintElement
    {
        public ElementSetConstraintElement(ConstraintElement constraintElement)
        {
            Element = constraintElement;
        }
    }
}