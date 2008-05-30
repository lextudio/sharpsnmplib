/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/17
 * Time: 17:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
	public class MibDocument
	{
		public MibDocument(Lexer lexer)
		{
			Symbol temp;
			while ((temp = lexer.NextSymbol) != null)
			{
                if (temp == Symbol.EOL)
                {
                    continue;
                }
				_modules.Add(new MibModule(temp.ToString(), lexer));                
			}
		}
		
		IList<MibModule> _modules = new List<MibModule>();
		
		public IList<MibModule> Modules
		{
			get
			{
				return _modules;
			}
		}
	}
}
