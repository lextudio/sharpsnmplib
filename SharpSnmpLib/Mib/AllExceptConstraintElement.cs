namespace Lextm.SharpSnmpLib.Mib
{
    public class AllExceptConstraintElement : ConstraintElement
    {
        public AllExceptConstraintElement(ConstraintElement constraintElement)
        {
            Element= constraintElement;
        }
    }
}