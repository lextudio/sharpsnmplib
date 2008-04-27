using System;
using System.Collections.Generic;
using System.Text;
using X690;

namespace SharpSnmpLib
{
    public class Timeticks: Integer
    {
        public Timeticks(byte[] bytes)
            : base(bytes)
        { }
    }
}
