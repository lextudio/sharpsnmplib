using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class FullQualifiedValue : ISmiValue
    {
        private readonly IList<string> _list = new List<string>();

        public void Add(string part)
        {
            _list.Add(part);
        }
    }
}
