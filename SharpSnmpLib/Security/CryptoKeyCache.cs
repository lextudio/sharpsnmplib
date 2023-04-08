using System.Text;

namespace Lextm.SharpSnmpLib.Security
{
    /// <summary>
    /// Class for holding computed crypto values which are referenced by password/engineId combination
    /// This class is not thread safe, it does not contain any static parameters.
    /// </summary>
    public class CryptoKeyCache
    {
        /// <summary>
        /// Number of elements that Cache will hold before deleting old elements
        /// </summary>
        private const int CacheCapacity = 100;

        #region InternalClasses
        /// <summary>
        /// Class for holding cached crypto keys computed values, since every password/engine id 
        /// combination will produce a different key this class is modeled using
        /// Dictionary of Dictionaries. This class is not thread safe.
        /// </summary>
        private sealed class EngineIdCache
        {
            /// <summary>
            /// Cache to map engineId to keys
            /// </summary>
            private readonly Cache<string, byte[]> _engineIdCache;

            /// <summary>
            /// Default ctor initializes EngineIdCache
            /// </summary>
            public EngineIdCache(int capacity)
            {
                _engineIdCache = new Cache<string, byte[]>(capacity);
            }


            /// <summary>
            /// Gets the cached value associated with the specified key.
            /// </summary>
            /// <param name="engineId"> The engineId of the cached value to get.</param>
            /// <param name="cachedValue">
            ///  When this method returns, contains the cachedValue associated with the specified
            ///  engineId, if the engineId is found; otherwise, the default value for the type of the
            ///  cachedValue parameter. This parameter is passed uninitialized.
            /// </param>
            /// <returns> True if the cache contains an element with the specified engineId; otherwise, false.</returns>
            public bool TryGetCachedValue(byte[] engineId, out byte[]? cachedValue)
            {
                return _engineIdCache.TryGetValue(Stringanize(engineId), out cachedValue);
            }

            /// <summary>
            /// Adds value to cache
            /// </summary>
            /// <param name="engineId">engine id associated with the value</param>
            /// <param name="valueToCache">value to cache</param>
            public void AddValueToCache(byte[] engineId, byte[] valueToCache)
            {
                _engineIdCache.Add(Stringanize(engineId), valueToCache);
            }
        }
        #endregion

        private readonly Cache<string, EngineIdCache> _cryptoCache;

        /// <summary>
        /// Ctor
        /// </summary>
        public CryptoKeyCache(int capacity)
        {
            _cryptoCache = new Cache<string, EngineIdCache>(capacity == 0 ? CacheCapacity : capacity);
        }

        /// <summary>
        /// Get the cached value if it exists in the cache
        /// </summary>
        /// <param name="password">password associated with cached value</param>
        /// <param name="engineId">engine id associated with cached value</param>
        /// <param name="cachedValue">cached value, if no cache exists for specified password/engine id 
        /// combination default value is assigned to cachedValue </param>
        /// <returns>True if value exists in cache for specified password/engine id combination, false otherwise</returns>
        public bool TryGetCachedValue(byte[] password, byte[] engineId, out byte[]? cachedValue)
        {
            string strPassword = Stringanize(password);
            cachedValue = null;
            bool success = _cryptoCache.TryGetValue(strPassword, out var engineCache);
            if (success)
            {
                success = engineCache!.TryGetCachedValue(engineId, out cachedValue);
            }

            return success;
        }

        /// <summary>
        /// Adds computed value to the cache
        /// </summary>
        /// <param name="password">password to associate cached value with </param>
        /// <param name="engineId">engine id to associate cached value with</param>
        /// <param name="valueToCache">value being cached</param>
        public void AddValueToCache(byte[] password, byte[] engineId, byte[] valueToCache)
        {
            string strPassword = Stringanize(password);
            if (!_cryptoCache.ContainsKey(strPassword))
            {
                _cryptoCache.Add(strPassword, new EngineIdCache(CacheCapacity));
            }

            EngineIdCache engineCache = _cryptoCache[strPassword];
            engineCache.AddValueToCache(engineId, valueToCache);
        }

        /// <summary>
        /// Converts an array of bytes into a string this way we can use
        /// string.GetHashCode and string.Equals to allow the array of bytes 
        /// be the key in a hash table
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string Stringanize(byte[] bytes)
        {
            StringBuilder builder = new();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString());
            }
            return builder.ToString();
        }
    }
}
