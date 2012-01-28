using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class ModuleCompliance {
        private readonly IList<ISmiValue> _mandarotyGroups = new List<ISmiValue>();
        public IList<Compliance> Compliances = new List<Compliance>();
        public ISmiValue Value;
        public IList<ISmiValue> MandatoryGroups = new List<ISmiValue>();

        public string Name { get; set; }

        public IList<ISmiValue> MandarotyGroups
        {
            get {
                return _mandarotyGroups;
            }
        }
    }
}