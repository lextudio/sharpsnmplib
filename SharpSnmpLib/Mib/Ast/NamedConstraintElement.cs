using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class NamedConstraintElement : ConstraintElement
    {
        public bool Present;
        public bool Absent;
        public bool Optinal;
        public string Name { get; set; }

        public Constraint Constraint { get; set; }

        public NamedConstraintElement(string name)
        {
            Name = name;
        }
    }
}