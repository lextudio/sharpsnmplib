
namespace Lextm.SharpSnmpLib.Mib
{
    internal class ValueRange
    {
        private int _first;
        private int? _second;

        public ValueRange(Symbol first, Symbol second)
        {
            int value;

            if (int.TryParse(first.ToString(), out value))
            {
                _first = value;
            }
            else
            {
                var parts = first.ToString().Split(new char[] { '.' });
                first.Validate(!(parts.Length == 3 && string.IsNullOrEmpty(parts[1])), "illegal sub-typing");
                first.Validate(!int.TryParse(parts[0], out value), "illegal sub-typing");
                _first = value;
                first.Validate(!int.TryParse(parts[2], out value), "illegal sub-typing");
                _second = value;
                first.Validate(_first >= _second, "illegal sub-typing");
            }

            if (second != null && int.TryParse(second.ToString(), out value))
            {
                _second = value;
                second.Validate(_first >= _second, "illegal sub-typing");
            }
            else
            {
                _second = null;
            }
        }

        internal bool Contains(int p)
        {
            if (_second == null)
            {
                return p == _first;
            }
            else
            {
                return _first <= p && p <= _second;
            }
        }
    }
}
