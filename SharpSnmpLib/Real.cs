using System;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Text;

// ASN.1 BER encoding library by Malcolm Crowe at the University of the West of Scotland
// See http://cis.paisley.ac.uk/crow-ci0
// This is version 0 of the library, please advise me about any bugs
// mailto:malcolm.crowe@paisley.ac.uk

// Restrictions: It is assumed that no encoding has a length greater than 2^31-1.
// UNIVERSAL TYPES
// Some of the more unusual Universal encodings are supported but not fully implemented
// Should you require these types, as an alternative to changing this code
// you can catch the exception that is thrown and examine the contents yourself.
// APPLICATION TYPES
// If you want to handle Application types systematically, you can derive a class from
// Universal, and provide the Creator and Creators methods for your class
// You will see an example of how to do this in the Snmplib
// CONTEXT AND PRIVATE TYPES
// Ad hoc coding can be used for these, as an alterative to derive a class as above.

namespace X690
{
	public class Real
	{
		public byte[] octVal = null;
		public Real(byte[] b)
		{
			octVal = b; 
		}
		public Real(double b)
		{
			if (b==0.0) 
			{
				octVal = new byte[0];
				return;
			}
			string s = b.ToString("E"); // hope this is acceptable..
			octVal = new byte[s.Length+1];
			octVal[0] = 0x0;
			new ASCIIEncoding().GetBytes(s,0,s.Length,octVal,1);
		}
		static public implicit operator double(Real x)
		{
			if (x.octVal.Length==0)
				return 0.0;
	            if ((x.octVal[0] & 0x80) != 0) // 8.5.5 binary encoding
	            {
	                byte c = x.octVal[0];
	                int s = ((c & 0x40) != 0) ? -1 : 1;
	                int t = c & 0x30;
	                int b = (t == 0) ? 2 : (t == 1) ? 8 : 16;
	                if (t == 3)
	                    throw (new Exception("X690:8.5.5.2 reserved encoding"));
	                int f = (c & 0xc) >> 2;
	                f = 1 << f;
	                int p = 1;
	                int h = (c & 0x3) + 1;
	                if (h == 4)
	                    h = x.octVal[p++];
	                byte[] g = new byte[h];
	                for (int j = 0; j < h; j++)
	                    g[j] = x.octVal[p++];
	                bool eb = CheckTwosComp(g);
	                long e = new Integer(g);
	                if (eb)
	                    e = -e;
	                byte[] m = new byte[x.octVal.Length - p];
	                for (int k = 0; p < x.octVal.Length; k++, p++)
	                    m[k] = x.octVal[p];
	                long n = new Integer(m);
	                return ((double)(s * n * f)) * Math.Pow((double)b, (double)e);
	            }
	            else if ((x.octVal[0] & 0x40) == 0) // 8.5.6 decimal encoding
	                return double.Parse(new ASCIIEncoding().GetString(x.octVal, 0, x.octVal.Length));
	            else // 8.5.7 special real encoding
	                switch (x.octVal[0])
	                {
	                    case 0x40: return double.PositiveInfinity;
	                    case 0x41: return double.NegativeInfinity;
	                    default: throw (new Exception("X690:8.5.7 reserved encoding"));
	                }
		}
	        static bool CheckTwosComp(byte[] b)
	        {
	            if (b[0] < 128)
	                return false;
	            int c = 1;
	            for (int j = b.Length - 1; j >= 0; j--)
	            {
	                if (b[j] == 0 && c > 0)
	                    continue;
	                b[j] = (byte)(255 - b[j] + c);
	                c = 0;
	            }
	            return true;
	        }
	        public override string ToString()
		{
			return ((double)this).ToString();
		}
	}
	// all references here are to ITU-X.690-12/97
}
