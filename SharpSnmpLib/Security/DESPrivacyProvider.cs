using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Security
{
    public class DESPrivacyProvider : IPrivacyProvider
    {
        #region IPrivacyProvider Members

        public string Name
        {
            get { return "DES"; }
        }

        public Sequence Decrypt(ISnmpData data)
        {
            OctetString octets = (OctetString)data;
            byte[] bytes = octets.GetRaw();
            return new Sequence(bytes);
        }

        #endregion
    }
}
