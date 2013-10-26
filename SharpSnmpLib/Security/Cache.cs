using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Collection for improving performance. Using hashing of key/value pairs.
    /// Oldest elements will be removed from the Cache when the capacity of the cache is reached.
    /// This class is not thread safe.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class Cache<TKey, TValue>
    {
        #region Data

        private readonly Dictionary<TKey, TValue> _dictionary;
        private readonly Queue<TKey> _keyQueue;
        private readonly int _capacity;

        #endregion //Data

        #region Public_Properties

        /// <summary>
        /// Gets the number of key/value pairs contained in the Cache.
        /// </summary>
        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }

        #endregion //Public_Properties

        #region Public_Methods

        /// <summary>
        /// Caching class for improving performance. Oldest elements are removed as the 
        /// cache is filled up
        /// </summary>
        /// <param name="initialCapacity">Capacity of the cache before oldest elements start to get removed</param>
        public Cache(int initialCapacity)
        {
            _dictionary = new Dictionary<TKey, TValue>(initialCapacity);
            _keyQueue = new Queue<TKey>(initialCapacity);
            _capacity = initialCapacity;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, 
        /// if the key is found; otherwise, the default value for the type of the value parameter.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if the Cache contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Determines whether the Cache contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the Cache</param>
        /// <returns>true if the Cache contains an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <exception cref="System.ArgumentNullException"> key is null.</exception> 
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">The property is retrieved and key does not exist in the collection.</exception>
        /// <returns>The value associated with the specified key.
        ///  If the specified key is not found, a get operation throws a System.Collections.Generic.KeyNotFoundException,
        ///  and a set operation creates a new element with the specified key.</returns>
        public TValue this[TKey key]
        {
            get { return _dictionary[key]; }
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary. If the cache has reached 
        /// its capacity oldest element will be removed automatically 
        /// </summary>
        /// <exception cref="System.ArgumentNullException">key is null</exception>
        /// <exception cref="System.ArgumentException">An element with the same key already exists in the Cache</exception>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        public void Add(TKey key, TValue value)
        {
            if (IsCacheFull())
            {
                RemoveOldestElement();
            }

            _dictionary.Add(key, value);            //Order of adding is important since dictionary can throw System.ArgumentNullException or System.ArgumentException
            _keyQueue.Enqueue(key);
        }

        #endregion //Public_Methods

        #region Private_Methods

        /// <summary>
        /// Removes oldest element from the cache
        /// </summary>
        private void RemoveOldestElement()
        {
            TKey keyToRemove = _keyQueue.Dequeue();
            _dictionary.Remove(keyToRemove);
        }

        /// <summary>
        /// Checks whether cache size has reached the capacity
        /// </summary>
        /// <returns>True if reached capacity false otherwise</returns>
        private bool IsCacheFull()
        {
            return _keyQueue.Count() >= _capacity;      //using >= instead of == in case someone doesn't syncronize Cache
        }

        #endregion //Private_Methods
    }
}
