using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    internal interface ISnmpObject
    {
        ISnmpData Get();

        void Set(ISnmpData data);

        ObjectIdentifier Id { get; }
    }
}
