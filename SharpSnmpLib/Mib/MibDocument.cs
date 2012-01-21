using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class MibDocument
    {
        private readonly IList<MibModule> _modules = new List<MibModule>();
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set 
            { 
                _fileName = value;
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                foreach (var module in Modules)
                {
                    module.FileName = value;
                }
            }
        }

        public IList<MibModule> Modules
        {
            get { return _modules; }
        }

        public void Add(MibModule module)
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                module.FileName = FileName;
            }

            _modules.Add(module);
        }
    }
}
