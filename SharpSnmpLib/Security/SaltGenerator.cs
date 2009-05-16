using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Security
{
    internal class SaltGenerator
    {
        private long _salt = Convert.ToInt64(new Random().Next(1, int.MaxValue));

        /// <summary>
        /// Get next salt Int64 value. Used internally to encrypt data.
        /// </summary>
        /// <returns>Random Int64 value</returns>
        private long NextSalt()
        {
            if (_salt == Int64.MaxValue)
            {
                _salt = 1;
            }
            else
            {
                _salt++;
            }

            return _salt;
        }

        public byte[] GetSaltBytes()
        {
            byte[] buffer = BitConverter.GetBytes(NextSalt());
            Array.Reverse(buffer);
            return buffer;
        }
    }
}
