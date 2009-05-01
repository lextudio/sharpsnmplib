using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Security
{
    public interface IAuthenticationProvider
    {
        string Name
        {
            get;
        }
    }
}
