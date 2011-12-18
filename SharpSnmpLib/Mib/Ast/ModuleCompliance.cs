using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class ModuleCompliance {
        private readonly IList<ISmiValue> _mandarotyGroups = new List<ISmiValue>();
        public IList<Compliance> Compliances;
        public ISmiValue Value;
        public IList<ISmiValue> MandatoryGroups;

        public string ModuleName { get; set; }

        public IList<ISmiValue> MandarotyGroups
        {
            get {
                return _mandarotyGroups;
            }
        }

        public ModuleCompliance(string name)
        {
            ModuleName = name;
        }
    }
}