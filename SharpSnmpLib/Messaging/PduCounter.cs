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
        /// Initializes a new instance of the <see cref="IdGenerator"/> class.
        /// </summary>
        /// <param name="min">The min.</param>
        /// <param name="max">The max.</param>
        public IdGenerator(int min, int max)
        {
            _min = min;
            _max = max;
            _salt = new Random().Next(_min, _max);
        }

        /// <summary>
        /// Returns next ID.
        /// </summary>
        public int NextId
        {
            get
            {
                lock (_root)
                {
                    if (_salt == _max)
                    {
                        _salt = _min;
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
        private int _salt;
        private readonly int _min;
        private readonly int _max;
    }
}
