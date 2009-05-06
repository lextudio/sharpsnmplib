using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Security
{
    public class MD5AuthenticationProvider : IAuthenticationProvider
    {
        private static MD5AuthenticationProvider instance;
        private static object root = new object();

        public static MD5AuthenticationProvider Instance
        {
            get
            {
                lock (root)
                {
                    if (instance == null)
                    {
                        instance = new MD5AuthenticationProvider();
                    }
                }

                return instance;
            }
        }

        #region IAuthenticationProvider Members

        public string Name
        {
            get { return "MD5 authentication provider"; }
        }

        public SecurityParameters Decrypt(ISnmpData data)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
