using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    public class DefaultModule : IModule
    {
        public DefaultModule(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public IList<IEntity> Objects { get; private set; }
        public IList<IEntity> Entities { get; private set; }
        public IList<string> Dependents { get; private set; }
        public string FileName { get; private set; }
    }
}