using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Security
{
    public sealed class DefaultPrivacyProvider : IPrivacyProvider
    {
        private DefaultPrivacyProvider()
        {
        }

        private static IPrivacyProvider _instance;
        private static object root = new object();

        public static IPrivacyProvider Instance
        {
            get
            {
                lock (root)
                {
                    if (_instance == null)
                    {
                        _instance = new DefaultPrivacyProvider();
                    }
                }

                return _instance;
            }
        }

        #region IPrivacyProvider Members

        public string Name
        {
            get { return "Default: NoPriv"; }
        }

        public Sequence Decrypt(ISnmpData data)
        {
            return (Sequence)data;
        }

        #endregion
    }
}
