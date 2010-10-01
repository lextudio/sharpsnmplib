*Homepage*

#SNMP binaries and source code are available at http://sharpsnmplib.codeplex.com

*License*

The #SNMP Library source code (for SharpSnmpLib*.dll) is released under Less GPL 2.1.

The #SNMP MIB Browser source code (Browser.exe) is released under MIT/X11 License.

The #SNMP MIB Compiler source code (Compiler.exe and related) is released under MIT/X11 License.

The #SNMP Agent source code (snmpd.exe) is released under MIT/X11 License. 

Other demo source code is released in public domain.

*Build From Source Code*

For .NET 4 (recommended)
1. Install .NET Framework 4.0 (http://www.microsoft.com/downloads/details.aspx?FamilyID=9cfb2d51-5ff4-4491-b0e5-b386f32c0992&displaylang=en).
2. Install .NET SDK or Visual Studio.
3. Execute prepare.bat to prepare necessary files.
4. Execute debug.bat to build the source code.

For .NET 3.5
1. Install .NET Framework 3.5 SP1 (http://www.microsoft.com/downloads/details.aspx?familyid=AB99342F-5D1A-413D-8319-81DA479AB0D7&displaylang=en).
2. Install .NET SDK or Visual Studio.
3. Execute prepare.bat to prepare necessary files.
4. Execute debug.35.bat to build the source code.

For Mono (on openSUSE)
1. Install Mono 2.* (http://www.go-mono.com/mono-downloads/download.html).
2. Use terminal to execute prepare.sh to prepare necessary files.
3. Execute debug.sh to build the source code.

For Mono (on Ubuntu 10.10)
1. Execute the following in a terminal, 
   sudo apt-get install mono-devel mono-vbnc mono-xbuild monodevelop
2. Open Properties for prepare.sh.
3. In Permissions tab, check the box for Allow executing file as program and click Close.
4. Do the same for debug.sh.
5. Execute prepare.sh in the terminal.
6. Execute debug.sh to build the source code.

*Notes*
1. VB.NET projects cannot be compiled under Mono at this moment.
2. WinForms applications are not recommended to be used in Visual Studio 2008. Please switch to Visual Studio 2010.