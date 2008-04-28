/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace SharpSnmpLib
{
	public class SharpSnmpException: Exception
	{
        public SharpSnmpException(string message) : base(message) { }
        public SharpSnmpException(string message, Exception inner) : base(message, inner) { }
	}

}