namespace Lextm.SharpSnmpLib.Mib.Ast
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