
namespace Lextm.SharpSnmpLib.Mib
{
    internal class ValueRange
    {
        private readonly int _start;
        private readonly int? _end;

        public ValueRange(Symbol first, Symbol second)
        {
            int value;

            if (int.TryParse(first.ToString(), out value))
            {
                _start = value;
            }
            else
            {
                var parts = first.ToString().Split(new char[] { '.' });
                first.Validate(!(parts.Length == 3 && string.IsNullOrEmpty(parts[1])), "illegal sub-typing");
                first.Validate(!int.TryParse(parts[0], out value), "illegal sub-typing");
                _start = value;
                first.Validate(!int.TryParse(parts[2], out value), "illegal sub-typing");
                _end = value;
                first.Validate(_start >= _end, "illegal sub-typing");
            }

            if (second != null && int.TryParse(second.ToString(), out value))
            {
                _end = value;
                second.Validate(_start >= _end, "illegal sub-typing");
            }
            else
            {
                _end = null;
            }
        }

        public int Start
        {
            get { return _start; }
        }

        public int? End
        {
            get { return _end; }
        }

        internal bool Contains(int p)
        {
            if (_end == null)
            {
                return p == _start;
            }
            else
            {
                return _start <= p && p <= _end;
            }
        }
    }
}
