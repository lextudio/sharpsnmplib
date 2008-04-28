using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Snmp;
using X690;

namespace SharpSnmpLib
{
    public class GetMessage
    {
        public GetMessage(IPAddress agent, string community, Variable variable)
        {
            _agent = agent;
            _community = community;
            _variable = variable;
            _sess = new ManagerSession(_agent, _community);
        }

        public Variable Send(int timeout)
        {
            _sess.Timeout = timeout;
            
            ManagerItem mi = new ManagerItem(_sess, _variable.Id.ToOid());
            _variable.Data = mi.Value;
            return _variable;
        }
        ManagerSession _sess;
        
        public byte[] ToBytes()
        {
        	return _sess.GetBytes(_sess.VarBind(_variable.Id.ToOid()));
        }

        IPAddress _agent;
        string _community;
        Variable _variable;
    }
}
