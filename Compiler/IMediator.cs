using System;
using System.Collections.Generic;
using System.Text;
using Lextm.SharpSnmpLib.Mib;

namespace Lextm.SharpSnmpLib.Compiler
{
    interface IMediator
    {
        IOutput Output
        {
            get;
        }

        IObjectTree Tree
        {
            get;
        }
    }
}
