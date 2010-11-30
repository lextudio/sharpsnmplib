
namespace Lextm.SharpSnmpLib.Mib
{
    internal class ValueRange
    {
        private int _first;
        private int? _second;

        public ValueRange(Symbol first, Symbol second)
        {
            int value1, value2;

            if (int.TryParse(first.ToString(), out value1))
            {
                _first = value1;
            }

            if (int.TryParse(second.ToString(), out value2))
            {
                _second = value2;
                second.Validate(_first >= _second, "illegal sub-typing");
            }
            else
            {
                _second = null;
            }
        }
    }
}
