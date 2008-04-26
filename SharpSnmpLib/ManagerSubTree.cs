
using System;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Text;
using System.Net;
using X690;

// SNMP library for .NET by Malcolm Crowe at University of the West of Scotland
// http://cis.paisley.ac.uk/crow-ci0/
// This is version 0 of the library. Email bugs to
// mailto:malcolm.crowe@paisley.ac.uk

// Getting Started
// The simplest way to get an SNMP value from a host is
// ManagerItem mi = new ManagerItem(
//								new ManagerSession(hostname,"public"),
//								"1.3.6.1.2.1.1.4.0");
// Then the actual OID is mi.Name and the value is in mi.Value.ToString().

// TODO: Tables, lists of bindings
//		 Friendly strings derived from MIBs

namespace Snmp
{





	public class ManagerSubTree
	{
		protected ManagerSession sess;
		ArrayList values = new ArrayList(); // of Universal (Variable Binding) 0:OID 1:Value
		uint[] start = null;
		public ManagerSubTree(ManagerSession s,uint[] oid)
		{
			sess = s;
			start = oid;
			Refresh();
		}
		bool Match(uint[] a, uint[] b) // check that b starts with a
		{
			if (b.Length<a.Length)
				return false;
			for (int j=0;j<a.Length;j++)
				if (a[j]!=b[j])
					return false;
			return true;
		}
		public void Refresh()
		{
			Universal v = sess.VarBind(start);
			values.Clear();
			uint[] next = start;
			bool fix = false;
			while (sess.GetNext(ref v)) 
			{
				uint[] from = (uint[])v[0].Value;
				if (!Match(start,from)) // outside the start subtree
					break;
				// the SNMP standard says that from should be lexicographically greater than next
				if (from.Length<=next.Length && Match(from,next))   // non-compliance
				{
					if (fix||(from.Length<next.Length)) // already tried to fix it or agent really bad
						throw(new Exception("Non-compliant agent"));
					fix = true; // try as a fix to add .0 on the end
					next = new uint[from.Length+1];
					for (int j=0;j<from.Length;j++)
						next[j] = from[j];
					next[from.Length] = 0; // and try again
				} 
				else // compliant: add it to the result set
				{
					values.Add(v);
					next = from;
					fix = false;
				}
				v = sess.VarBind(next);
			}
		}
		public int Length { get { return values.Count; }}
		public ManagerItem this[int x] { get { return new ManagerItem(sess,(Universal)values[x]); }}
		public _Enumerator GetEnumerator()
		{
			return new _Enumerator(this);
		}
		public class _Enumerator 
		{
			ManagerSubTree st;
			int ix = -1;
			public _Enumerator(ManagerSubTree t) { st=t; }
			public bool MoveNext()
			{
				if (ix==st.Length-1)
					return false;
				ix++;
				return true;
			}
			public ManagerItem Current { get { return st[ix]; } }
			public void Reset() { ix = -1; }
		}
	}
}