/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 11/29/2009
 * Time: 11:01 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Agent
{
	internal interface IMessageHandler
	{
        IList<Variable> Handle(ISnmpMessage message, ObjectStore store);
		ErrorCode ErrorStatus { get; }
		int ErrorIndex { get; }
	}
}
