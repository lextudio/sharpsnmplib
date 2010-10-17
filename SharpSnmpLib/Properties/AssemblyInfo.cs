// <summary>#SNMP Library. An open source SNMP implementation for .NET.</summary>
// <copyright company="Lex Y. Li" file="AssemblyInfo.cs">Copyright (C) 2008  Lex Y. Li</copyright>
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

#region Using directives

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SharpSnmpLib")]
[assembly: AssemblyDescription("#SNMP Library for .NET")]
[assembly: AssemblyConfiguration("Less GPL")]
[assembly: AssemblyCompany("LeXtudio")]
[assembly: AssemblyProduct("#SNMPLib")]
[assembly: AssemblyCopyright("(C) 2008-2009 Malcolm Crowe, Lex Li, Steve Santacroce, and other contributors.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// This sets the default COM visibility of types in the assembly to invisible.
// If you need to expose index type to COM, use [ComVisible(true)] on that type.
[assembly: ComVisible(false)]

// The assembly version has following format :
//
// Major.Minor.Build.Revision
//
// You can specify all the values or you can use the default the Revision and
// Build Numbers by using the '*' as shown below:
[assembly: AssemblyVersion("6.0.011017.42")]
#if (!CF)
[assembly: AssemblyFileVersion("6.0.011017.42")]
#endif
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: CLSCompliant(true)]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Lextm")]

[assembly: InternalsVisibleTo("SharpSnmpLib.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000011000000d9a9ad86f872e6"
+ "46aeb279fc116b6bfb4902fd43b59d044449a0f22fd4a606f35a55784fec360a71472f1af0c35f"
+ "111ae1678f8d454328c53b31a0a81210c08fa7e22d4f8ad089e4d9985551e864ddda83e0fda733"
+ "567a5af7f1b5d181a7c141833956073b4cb491684bc2e7150d9a62baf03f71fa9203afb6c4d3bc"
+ "edceb394")]
