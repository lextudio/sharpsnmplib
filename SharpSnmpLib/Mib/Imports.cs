/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/31
 * Time: 12:07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
	/// <summary>
	/// The IMPORTS construct is used to specify items used in the current MIB module which are defined in another MIB module or ASN.1 module.
	/// </summary>
	sealed class Imports : IConstruct
	{
		IList<string> _dependents = new List<string>();
		/// <summary>
		/// Creates an <see cref="Imports"/> instance.
		/// </summary>
		/// <param name="lexer"></param>
		public Imports(Lexer lexer)
		{
			Symbol temp;
			while ((temp = lexer.NextSymbol) != Symbol.Semicolon)
			{
                if (temp == Symbol.EOL)
                {
                    continue;
                }
				_dependents.Add(new ImportsFrom(temp, lexer).Module);
			}
		}
		
		internal IList<string> Dependents
		{
			get
			{
				return _dependents;
			}
		}		
	}
	

}
