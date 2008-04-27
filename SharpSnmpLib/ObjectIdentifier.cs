using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SharpSnmpLib
{
    public struct ObjectIdentifier
    {
        /// <summary>
        /// Creates an <see cref="ObjectIdentifier"/> instance from OID array.
        /// </summary>
        /// <param name="oid">OID <see cref="uint"/> array</param>
        public ObjectIdentifier(uint[] oid)
        {
            _oid = oid;
        }
        /// <summary>
        /// Creates an <see cref="ObjectIdentifier"/> instance from PDU raw bytes.
        /// </summary>
        /// <param name="bytes">PDU raw bytes</param>
        /// <param name="n">number</param>
        public ObjectIdentifier(byte[] bytes)
        {
            _oid = ParseRawBytesFromPdu(bytes, (uint)bytes.Length);
        }

        public uint[] ToOid()
        {
            return _oid;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int k = 0; k < _oid.Length; k++)
            {
                result.Append("." + _oid[k]);
            }
            return result.ToString();
        }

        uint[] _oid;
        /// <summary>
        /// Decodes PDU OID representation.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        static uint GetOIDEl(byte[] bytes, ref int p)
        {
            uint r = 0;
            byte x;
            do
            {
                x = bytes[p++];
                r = (r << 7) + ((uint)x & 0x7f);	// 0.3.3a (Evan Watson)
            } while ((x & 0x80) != 0);
            return r;
        }

        static uint[] ParseRawBytesFromPdu(byte[] bytes, uint n)
        {
            uint[] result;
            IList<uint> temp = new List<uint>();
            int p = 0;
            while (p < n)
            { temp.Add(ObjectIdentifier.GetOIDEl(bytes, ref p)); }

            if (temp[0] == 43)
            {
                result = new uint[temp.Count + 1];
                result[0] = 1;
                result[1] = 3;
                for (int j = 1; j < temp.Count; j++)
                { result[j + 1] = (uint)temp[j]; }
            }
            else // can happen that result is .0??
            {
                //				throw(new Exception("OID must begin with .1.3"));
                result = new uint[temp.Count];
                for (int j = 0; j < temp.Count; j++)
                { result[j] = (uint)temp[j]; }
            }
            return result;
        }
    }
}
