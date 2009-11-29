using System;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// A counter that generates ID.
    /// </summary>
    /// <remarks>The request ID is used to identifier sessions.</remarks>
    public sealed class IdGenerator
    {      
        /// <summary>
        /// Returns next ID.
        /// </summary>
        public int NextId
        {
            get
            {
                lock (_root)
                {
                    if (_salt == int.MaxValue)
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
        }

        private readonly object _root = new object();
        private int _salt = new Random().Next(1, int.MaxValue);
    }
}
