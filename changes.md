# Release Notes

## 13.0.0-beta.1 - Released on 2026-02-26

**NuGet Package**

[![NuGet Version](https://img.shields.io/nuget/vpre/Lextm.SharpSnmpLib)
](https://www.nuget.org/packages/Lextm.SharpSnmpLib/13.0.0-beta.1)

**Supported Platforms**

* .NET 8 and above.

**Changes Since 12.5.7**

* A partial compatibility layer is added for security providers and message APIs.
* New `MessageFactory.ParseMessages()` `throwOnV3SecurityError` parameter for flexible V3 message parsing.
* NuGet package now includes build/buildTransitive targets with common `Using` aliases.

**Breaking Changes**

* .NET Framework 4.7.1 support removed.
* Rebuilt on DotNetSnmp/System.Formats.Asn1 with reorganized types and namespaces.
