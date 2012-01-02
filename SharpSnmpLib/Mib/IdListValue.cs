using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class IdListValue : ISmiValue
    {
        private readonly IList<string> _ids = new List<string>(); 
        public void Add(string id)
        {
            _ids.Add(id);
        }
    }
}