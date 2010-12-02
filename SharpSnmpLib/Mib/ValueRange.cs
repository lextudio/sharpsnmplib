
namespace Lextm.SharpSnmpLib.Mib
{
    internal class ValueRange
    {
        private readonly int _start;
        private readonly int? _end;

        public ValueRange(Symbol first, Symbol second)
        {
            int value;

            first.Validate(!int.TryParse(first.ToString(), out value), "invalid sub-typing");
            _start = value;

            if (second != null)
            {
                second.Validate(!int.TryParse(second.ToString(), out value), "invalid sub-typing");
                _end = value;
                second.Validate(_start >= _end, "illegal sub-typing");
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
