
using SharpSnmpLib;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
	public class ManagerItem
	{
		bool isCached = false;
		protected ManagerSession sess;
	        public Universal Value
	        {
                get
                {
                    if (isCached)
                        return varbind[1];
                    varbind = sess.Get(sess.VarBind(Oid))[0];
                    isCached = true;
                    return varbind[1];
                }
	            set
	            {
	                // crash if ManagerItem does not already contain a binding
	                varbind[1] = value;
	                isCached = true;
	                sess.Set(varbind);
	            }
	        }
		public ObjectIdentifier Oid 
		{
			get 
			{
				return (ObjectIdentifier)(varbind[0].Value); 
			}
		}
		Universal varbind;
		public ManagerItem(ManagerSession s, ObjectIdentifier oid): this (s, s.VarBind(oid)) {}
		public ManagerItem(ManagerSession s,uint[] oid) : this (s,new ObjectIdentifier(oid)) {}
		public ManagerItem(ManagerSession s,Universal v)
		{
			sess = s;
			varbind = v;
		}
	        public void Refresh()
	        {
	            isCached = false;
	        }
	}
}
