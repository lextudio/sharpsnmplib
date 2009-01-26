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
[assembly: AssemblyDescription("C# SNMP Library")]
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
[assembly: AssemblyVersion("1.6.010126.55")]
#if (!CF)
[assembly: AssemblyFileVersion("1.6.010126.55")]
#endif
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: CLSCompliant(true)]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mib")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Lextm")]

[assembly: InternalsVisibleTo("SharpSnmpLib.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100cda8c7bee95df6" +
                              "7f8d98cc94c28b38249c0fd3e42f909d339b1d668dcc5dd1746deae7a097ae275b3a56ba9d2897" +
                              "0b5e9a803a0ca3ad0d6f9147c48a0fd103ad127e9e000b343e360a5430b70c6fd1a1358857592e" +
                              "0129e26e308c952967f5828448f19d8ac3355a8a907e34770e92ee051edcc315c2ad281fa6247b" +
                              "a7cd1dbd")]
