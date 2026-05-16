using System.Collections.Concurrent;

namespace Lextm.SharpSnmpLib.Security;

/// <summary>Simple bounded cache (legacy compatibility).</summary>
public class Cache<TKey, TValue> where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, TValue> _dict;
    private readonly int _capacity;

    /// <summary>Initializes a new instance with the specified capacity.</summary>
    public Cache(int capacity)
    {
        _capacity = capacity;
        _dict = new ConcurrentDictionary<TKey, TValue>();
    }

    /// <summary>Gets the number of cached items.</summary>
    public int Count => _dict.Count;

    /// <summary>Gets the value for the specified key.</summary>
    public TValue this[TKey key] => _dict[key];

    /// <summary>Tries to get a cached value.</summary>
    public bool TryGetValue(TKey key, out TValue value) => _dict.TryGetValue(key, out value!);

    /// <summary>Determines if the key exists.</summary>
    public bool ContainsKey(TKey key) => _dict.ContainsKey(key);

    /// <summary>Adds a value to the cache.</summary>
    public void Add(TKey key, TValue value)
    {
        if (_dict.Count < _capacity)
            _dict.TryAdd(key, value);
    }
}
