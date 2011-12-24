namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ElementSetConstraintElement : ConstraintElement
    {
        public ElementSetConstraintElement(ConstraintElement constraintElement)
        {
            Element = constraintElement;
        }
    }
}