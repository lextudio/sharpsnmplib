using System;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Salt generator.
    /// </summary>
    public sealed class SaltGenerator
    {
        private long _salt = Convert.ToInt64(new Random().Next(1, int.MaxValue));
        private readonly object _root = new object();

        /// <summary>
        /// Get next salt Int64 value. Used internally to encrypt data.
        /// </summary>
        /// <returns>Random Int64 value</returns>
        private long NextSalt()
        {
            lock (_root)
            {
                if (_salt == long.MaxValue)
                {
                    _salt = 1;
                }
                else
                {
                    _salt++;
                }
            }

            return _salt;
        }

        /// <summary>
        /// Gets salt bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] GetSaltBytes()
        {
            byte[] buffer = BitConverter.GetBytes(NextSalt());
            Array.Reverse(buffer);
            return buffer;
        }
    }
}
