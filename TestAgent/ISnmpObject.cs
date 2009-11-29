using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Agent
{
    internal interface ISnmpObject
    {
        ISnmpData Data { get; set; }
        ObjectIdentifier Id { get; }
    }
}
