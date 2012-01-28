using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class WithComponentsConstraintElement : ConstraintElement
    {
    	private IList<ConstraintElement> _list;
        public bool Ellipsis { get; set; }

        public IList<ConstraintElement> TypeConstraintList
        {
            get { return _list; }
            set { _list = value; }
        }
    }
}