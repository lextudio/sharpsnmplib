namespace Lextm.SharpSnmpLib.Security;

/// <summary>Cache for derived crypto keys (legacy compatibility).</summary>
public class CryptoKeyCache
{
    private readonly Cache<string, byte[]> _cache;

    /// <summary>Initializes a new instance.</summary>
    public CryptoKeyCache(int capacity)
    {
        _cache = new Cache<string, byte[]>(capacity);
    }

    /// <summary>Tries to retrieve a cached derived key.</summary>
    public bool TryGetCachedValue(byte[] password, byte[] engineId, out byte[] key)
    {
        var cacheKey = Convert.ToHexString(password) + "|" + Convert.ToHexString(engineId);
        return _cache.TryGetValue(cacheKey, out key!);
    }

    /// <summary>Adds a derived key to the cache.</summary>
    public void AddValueToCache(byte[] password, byte[] engineId, byte[] key)
    {
        var cacheKey = Convert.ToHexString(password) + "|" + Convert.ToHexString(engineId);
        _cache.Add(cacheKey, key);
    }
}
