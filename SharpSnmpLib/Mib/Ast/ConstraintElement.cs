namespace Lextm.SharpSnmpLib.Mib.Ast
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