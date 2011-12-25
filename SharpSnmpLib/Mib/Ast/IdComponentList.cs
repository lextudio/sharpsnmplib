using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib.Ast
{
    public class IdComponentList : ISmiValue
    {
        private readonly IList<IdComponent> _idComponents = new List<IdComponent>();
        public DefinedValue DefinedValue { get; set; }

        public IList<IdComponent> IdComponents
        {
            get { return _idComponents; }
        }

        public void Add(IdComponent idComponent)
        {
            IdComponents.Add(idComponent);
        }
    }
}