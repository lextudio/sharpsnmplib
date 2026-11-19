# Incompatibility Tracking for v13 Migration

This file tracks breaking changes between v12 and v13, root causes, affected types, and migration guidance.

## API parity status (v12.5 → v13)

Measured with `api-compare/api-parity` (run `dotnet run -c Release` in that folder; it auto-detects both builds and applies `parity-exclusions.json` at the workspace root). Last run: 2026-11-19, against a v12 Release `net8.0` build of `sharpsnmplib-12.0` and a fresh v13 Debug `net8.0` build of `sharpsnmplib-13.0` (branch `release-13.0`). The tool output is not checked in; rerun it to reproduce or refresh these numbers.

| Metric | Count |
|---|---|
| v12 public members | 1202 |
| Excluded (see below) | 165 |
| v12 members in scope | 1037 |
| Present in v13 | 1034 |
| Missing in v13 | 3 |
| **Parity score** | **99.7%** |

**Excluded members** are listed by pattern in `parity-exclusions.json`: low-level serialization plumbing that consumers did not call directly (`AppendBytesTo`, `Stream`/`Tuple<int, byte[]>` constructors, `SerializationInfo` constructors, enum `value__`) and pre-.NET 6 encryption/authentication helpers replaced by `SnmpV3Message`- and `Span<byte>`-based operations. They are not counted as breaks, but code calling them will not compile against v13.

**Missing members** — all three are intentional and documented in the sections below:

| v12 API | Source compatible? | Binary compatible? | Section |
|---|---|---|---|
| `Integer32.ToErrorCode()` (instance) | Yes — resolves to the extension method | No | [`Integer32.ToErrorCode()`](#code-fix-integer32toerrorcode-instance-method-removed) |
| `SnmpMessageExtension.IsRunningOnMono()` | No | No | [`IsRunningOnMono()`](#aottrim-snmpmessageextensionisrunningonmono-removed) |
| `SnmpDataExtension` (static class) | Only for extension-method calls | No | [`SnmpDataExtension`](#api-snmpdataextension-class-removed) |

**Binary compatibility in general**: v13 is not a drop-in binary replacement for v12. Core data types keep their v12 names and `Lextm.SharpSnmpLib` namespace, but several changed from reference types to value types — for example `Integer32`, `OctetString`, and `ObjectIdentifier` were `sealed class` in v12 and are `readonly record struct` in v13. Applications and libraries compiled against v12 must be recompiled against v13; mixing a v12-compiled dependency with the v13 package in one process is not supported. The class→struct change can also affect source that the parity tool does not flag, such as `null` checks or `null` assignments on these types.

## [AOT/Trim] `SnmpMessageExtension.IsRunningOnMono()` removed

- **Root cause**: it was the only reflection in v12 (`Type.GetType("Mono.Runtime")`) and forced `[RequiresUnreferencedCode]` onto the messaging API. v13 removes it to be AOT/trim-safe (see [`[RequiresUnreferencedCode]` annotations removed](#aottrim-requiresunreferencedcode-annotations-removed-in-v13)).
- **Affected types**: `SnmpMessageExtension`.
- **Migration guidance**: no replacement in the library. If you still need the check, inline it in your own code (`Type.GetType("Mono.Runtime") != null`), or use `RuntimeInformation.FrameworkDescription` / `OperatingSystem.Is*()` for platform decisions.

## [API] `SnmpDataExtension` class removed

- **Root cause**: v12's `SnmpDataExtension` held a single method, `ToBytes(this ISnmpData)`. In v13 that extension method lives in `CompatibilityExtensions` (same `Lextm.SharpSnmpLib` namespace), so the old class was not carried over.
- **Affected types**: `SnmpDataExtension`.
- **Migration guidance**:
  - `data.ToBytes()` (extension-method syntax) — no change; it now binds to `CompatibilityExtensions.ToBytes`.
  - `SnmpDataExtension.ToBytes(data)` (explicit static call) — change to `data.ToBytes()` or `CompatibilityExtensions.ToBytes(data)`.
  - Binaries compiled against v12 must be recompiled.

## [AOT/Trim] `[RequiresUnreferencedCode]` annotations removed in v13

- **Root cause**: v12 had 38 `[RequiresUnreferencedCode]` annotations on `Messenger`, `SnmpMessageExtension`, and `Discovery` methods. The only actual reflection was `Type.GetType("Mono.Runtime")` in `IsRunningOnMono()`. v13 eliminates all annotations because:
  - `IsRunningOnMono()` removed, replaced with trim-safe `OperatingSystem.Is*()` calls.
  - `SnmpMessageExtension.GetResponseAsync()` throws `NotImplementedException` (use `SnmpDispatcher.SendPdu` instead).
  - `SnmpDispatcher`-based architecture uses compile-time `switch` expressions, no reflection.
- **Affected types**: `Messenger`, `SnmpMessageExtension`, `Discovery`, `Net6Messenger`, `Net6SnmpMessageExtension`, `Net6Discovery`
- **Migration guidance**: Consumers targeting Native AOT in v13 should no longer see trim/AOT warnings from SharpSnmpLib. Library has `<IsAotCompatible>true</IsAotCompatible>` as of v13.
- **Verification**:
  - Library compiles with `<IsAotCompatible>true</IsAotCompatible>` — 0 AOT/trim errors.
  - `dotnet publish` with `<PublishAot>true</PublishAot>` on test project succeeds; native binary (15.8 MB arm64) produced.
  - No SharpSnmpLib-specific IL2XXX/IL3XXX warnings at build time.
  - xUnit v3 MTP runner has a known AOT limitation on macOS (`Assembly.Location` empty in single-file apps), preventing AOT test execution in this environment.

## [Code Fix] `Integer32.ToErrorCode()` instance method removed

- **Root cause**: `Integer32` struct had an instance method `ToErrorCode()` that did a plain cast `(ErrorCode)Value` (never throws). This shadowed the `CompatibilityExtensions.ToErrorCode(this Integer32)` extension method which validates the value and throws `InvalidCastException` for unknown error codes. The test `ToErrorCode_UnknownValue_Throws` expected the validation behavior but got the silent cast.
- **Affected types**: `Integer32` (instance method removed), `CompatibilityExtensions` (extension method now always invoked)
- **Migration guidance**: no source change needed — `value.ToErrorCode()` now binds to `CompatibilityExtensions.ToErrorCode`, which behaves like v12's `Integer32.ToErrorCode()`: both throw `InvalidCastException` for values outside the `ErrorCode` range. (The silent cast described above existed only in earlier v13 previews.) Binaries compiled against v12 must be recompiled, because the instance method no longer exists.

## [Test] MockTransport and AOT-compatible integration tests

- **Change**: Added `Tests/MockTransport.cs` — a lightweight `ISnmpTransport` implementation for offline testing. Supports both a response queue and a `ResponseBuilder` callback that parses the request and constructs matching responses.
- **Rewritten tests**: `IntegrationTest.TestMessengerGetAsyncV1_Mock`, `TestMessengerGetAsyncV2_Mock`, `TestMessengerSetAsyncV1_Mock`, `TestMessengerSetAsyncV2_Mock` — V1/V2 Get/Set operations verified via `MockTransport` without network access.
- **Original tests preserved**: Network-based integration tests (`TestMessengerGetAsyncV1`, etc.) remain for real device testing against `demo.pysnmp.com`.
- **AOT compatibility**: `MockTransport` uses no reflection, compiles clean under AOT.

## [Code Fix] Privacy key extension used `TruncatedDigestSize` instead of `DigestSize` (issue #705)

- **Root cause**: `TripleDESPrivacyProvider.ExtendShortKey` advanced its write cursor by `IAuthenticationProvider.TruncatedDigestSize` on each iteration, re-running password-to-key (P2K) each time. When the gap to fill exceeded `TruncatedDigestSize`, the loop ran a second iteration and copied the *leading octets of the same K2 block again* instead of the remaining octets. Per [draft-reeder-snmpv3-usm-3desede-00 §2.1 and Appendix B.1](https://datatracker.ietf.org/doc/html/draft-reeder-snmpv3-usm-3desede-00), key extension must chain the **full** P2K output: `K1 = P2K(passphrase)`, `K2 = P2K(K1)`, `localized = K1 || K2`. v12 already did this (it used `DigestLength`); v13 was a regression.
- **Affected types**: `TripleDESPrivacyProvider.ExtendShortKey` (`SharpSnmpLib/V3/Security/Privacy/TripleDESPrivacyProvider.cs`), and by shared use `AESPrivacyProviderBase` (`SharpSnmpLib/V3/Security/Privacy/AESPrivacyProviderBase.cs`, both the encrypt and decrypt paths).
- **Affected combinations** (only those where `keyLength - DigestSize > TruncatedDigestSize`):
  | Privacy | Auth | Gap | Affected |
  |---|---|---|---|
  | 3DES (32) | MD5 (16/12) | 16 | yes |
  | AES-256 (32) | MD5 (16/12) | 16 | yes |
  | AES-192 (24) | MD5 (16/12) | 8 | no |
  | AES-128 (16) | MD5 (16/12) | 0 | no |
  | 3DES / AES-* | SHA-1 (20/12) | ≤12 | no |
- **Impact**: the corrupted trailing octets are the pre-IV (3DES §5.1.1.1.2), so sender and receiver derived different IVs and 3DES+MD5 / AES-256+MD5 traffic could not be decrypted by a conformant peer. With MD5 the buggy 32-octet key ended in `...F44A90650EE0A3A40ABF79EF | F44A9065` instead of the draft's `...F44A90650EE0A3A40ABFAC5ACC12`.
- **Migration guidance**: none for callers — this restores v12 wire behavior. Any peer that interoperated with a v13 beta build's 3DES+MD5 or AES-256+MD5 output will no longer be able to decrypt it (or vice versa); v13 is still beta, so no compatibility shim is provided. See `Tests/ExtendShortKeyTest.cs` (draft Appendix B.1 vectors) and `Tests/PrivacyKeyChainingRoundTripTest.cs` (round-trip across all privacy/auth combinations).
- **Test note**: `MessageParsingTest.FullMessageTest_MD5_TripleDES` previously carried a hard-coded packet that had itself been produced by the non-conformant derivation (its `Scope` encoding quirk, including the `04 0E` length byte for a 13-octet engine ID, matches what this library emits). The vector was regenerated with the conformant implementation, so the test again decrypts through the normal `IPrivacyProvider.DecryptMessage` path.
