# SharpSnmpLib v13.0.0-beta.1 - Changes Compared to v12

This note summarizes major differences between v12 (`12.5.x`) and v13 beta line (`13.0.0-beta.1`).

## Highlights

1. v13 is rebuilt on top of the new DotNetSnmp core.
2. Public compatibility surface is preserved where practical via shim/facade types.
3. Runtime support moved forward to modern .NET only.

## Breaking Changes

1. Target frameworks changed:
   - v12: `net8.0` + `net471`
   - v13: `net8.0`, `net9.0`, `net10.0`
2. .NET Framework 4.7.1 support is removed in v13.
3. Internals are reorganized around DotNetSnmp types/namespaces, so reflection-based or internal-coupled integrations may require updates.

## Compatibility Layer Added in v13

1. Security provider facades in `Lextm.SharpSnmpLib.Security`:
   - `MD5AuthenticationProvider`, `SHA1AuthenticationProvider`, `SHA256AuthenticationProvider`, `SHA384AuthenticationProvider`, `SHA512AuthenticationProvider`
   - `DefaultPrivacyProvider`, `DESPrivacyProvider`, `AESPrivacyProvider`, `AES192PrivacyProvider`, `AES256PrivacyProvider`, `TripleDESPrivacyProvider`
   - legacy singleton and helper facades such as `DefaultAuthenticationProvider.Instance`
2. Message compatibility helpers in `SnmpMessageCompatibilityExtensions`:
   - `TypeCode()`, `Variables()`, `RequestId()`, `MessageId()`, `ToBytes()`, `Pdu()`
   - sync/async `GetResponse*` overloads (including socket and registry variants)
3. Privacy compatibility helpers in `PrivacyProviderCompatibilityExtensions`:
   - legacy-style `Encrypt/Decrypt` helper signatures
   - `ToSecurityLevel()` compatibility mapping
4. Common extension helpers in `CompatibilityExtensions`:
   - `ToInt32()`, `ToErrorCode()`, `GetRaw()`, `ToHexString()`, `ToBytes()`
5. NuGet package now includes build/buildTransitive targets with common `Using` aliases to reduce migration friction (including `ISnmpData` aliasing).

## Behavior Updates

1. `MessageFactory.ParseMessages(...)` has explicit `throwOnV3SecurityError` control.
   - Default parse path uses `false` to support agent-side report generation flow.
   - Manager/client response parsing paths can use `true` for strict behavior.
2. Timeout handling is standardized through compatibility paths; sync wrappers map wait timeouts to SNMP timeout exceptions.
3. Walk/BulkWalk and message APIs include cancellation-aware async overloads to better support modern async control flows.

## Migration Checklist (v12 -> v13)

1. Move applications to `.NET 8+` before adopting v13.
2. Rebuild and check for compile errors caused by removed .NET Framework support.
3. Keep using `Lextm.SharpSnmpLib.*` APIs first; use the compatibility shims where existing code depends on v12 signatures.
4. Review timeout/exception assertions in tests (especially sync vs async paths).
5. If you rely on internal types or reflection against old implementation details, update to supported public APIs.

## Notes

1. Some legacy compatibility types are marked `[Obsolete]` and intended as migration aids, not long-term contracts.
2. Additional parity and migration work continues in the v13 beta cycle.
