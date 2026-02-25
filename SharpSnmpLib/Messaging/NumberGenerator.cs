namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// A counter that generates IDs.
/// </summary>
public sealed class NumberGenerator
{
    private readonly object _root = new();
    private readonly int _min;
    private readonly int _max;
    private int _salt;

    /// <summary>
    /// Initializes a new instance of NumberGenerator.
    /// </summary>
    /// <param name="min">The minimum generated value.</param>
    /// <param name="max">The maximum generated value.</param>
    public NumberGenerator(int min, int max)
    {
        _min = min;
        _max = max;
        _salt = Random.Shared.Next(_min, _max);
    }

    /// <summary>
    /// Returns the next ID.
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

                return _salt;
            }
        }
    }

    internal void SetSalt(int value)
    {
        _salt = value;
    }
}
