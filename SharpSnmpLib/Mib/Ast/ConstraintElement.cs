using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public interface ConstraintElement {
        void Add(ConstraintElement element);
        ConstraintElement ConstraintElement { get; set; }
        ISmiType ConstraintType { get; set; }
        bool Includes { get; set; }
        bool Ellipsis { get; set; }
        IList<ConstraintElement> TypeConstraintList { get; set; }
    }
}