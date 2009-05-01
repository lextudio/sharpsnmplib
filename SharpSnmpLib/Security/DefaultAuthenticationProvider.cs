using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Security
{
    public sealed class DefaultAuthenticationProvider : IAuthenticationProvider
    {
        private DefaultAuthenticationProvider()
        {
        }

        private static object root = new object();
        private static IAuthenticationProvider _instance;

        public static IAuthenticationProvider Instance
        {
            get
            {
                lock (root)
                {
                    if (_instance == null)
                    {
                        _instance = new DefaultAuthenticationProvider();
                    }
                }

                return _instance;
            }
        }

        #region IAuthenticationProvider Members

        public string Name
        {
            get { return "Default: NoAuth"; }
        }

        #endregion
    }
}
