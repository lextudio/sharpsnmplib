namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class AllExceptConstraintElement : ConstraintElement
    {
        public AllExceptConstraintElement(ConstraintElement constraintElement)
        {
            Element= constraintElement;
        }
    }
}