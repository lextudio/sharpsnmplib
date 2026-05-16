# SharpSnmpLib v13 API Surface Policy

## Primary contract

The stable, consumer-facing API surface of `Lextm.SharpSnmpLib 13.x` is the `Lextm.SharpSnmpLib.*` namespace hierarchy — the same contract as v12. This is what NuGet consumers program against. Upgrading from v12 to v13 must not require source changes for the common API surface.

## DotNetSnmp namespace policy

`DotNetSnmp.*` namespaces are an internal implementation detail. They must not appear as the primary surface for consumers.

Rules:
- `DotNetSnmp.*` types that have no `Lextm.SharpSnmpLib.*` equivalent must be `internal`.
- `DotNetSnmp.*` types that are needed by consumers must be re-exported or aliased under `Lextm.SharpSnmpLib.*`.
- Do not direct consumers to use `DotNetSnmp.*` types directly; if a gap exists, add the type to `Lextm.SharpSnmpLib.*`.
- The `buildTransitive` targets file (`build/Lextm.SharpSnmpLib.targets`) may inject `global using` aliases to make `DotNetSnmp.*` types available unqualified, but this is a compatibility bridge — not a public contract.

## v12 API policy

v12 APIs are the primary stable surface. They must not be removed or marked `[Obsolete]` solely because v13 introduced a different internal implementation.

`[Obsolete]` is appropriate only when:
1. The API has a genuinely better replacement available in the same `Lextm.SharpSnmpLib.*` namespace (e.g., a constructor that lacks a required parameter like `contextName`).
2. The API reflects a protocol-level obsolescence (e.g., SNMP v2u, which the IETF marked obsolete).

`[Obsolete]` must not be used to:
- Mark a type as "internal use only" while keeping it public — make it `internal` instead.
- Guide consumers toward `DotNetSnmp.*` types as replacements.

## Type exposure mechanism

Core v12 types (`ObjectIdentifier`, `OctetString`, `Integer32`, `VersionCode`, etc.) live internally in `DotNetSnmp.Asn1.SyntaxObjects` and `DotNetSnmp.Common.Definitions`. They are made available to consumers via two mechanisms:

1. **Namespace-level `global using`** in `build/Lextm.SharpSnmpLib.targets` (injected transitively via NuGet) — makes types available unqualified in any consuming project.
2. **Short-name type aliases** in the same targets file — ensures `ObjectIdentifier`, `OctetString`, `VersionCode`, etc. resolve without needing an explicit `using DotNetSnmp.*` directive.

Fully-qualified `Lextm.SharpSnmpLib.ObjectIdentifier` references (as written by v12 consumers) require the consuming project to have `using Lextm.SharpSnmpLib;` in scope, which then resolves via the injected global alias. If this does not work in a specific consuming project, that is a regression — file an issue and fix the targets file.

## Regression gate

Before each release, verify:
- `USE_DOTNETSNMP_API` compile flag must not be needed in any `Lextm.SharpSnmpLib`-consuming project.
- All types listed in the v12 public API dump are still accessible under `Lextm.SharpSnmpLib.*` without any `using DotNetSnmp.*` directive.
- No v12 API has a new `[Obsolete]` attribute that was not present in v12 itself.

A `PublicApiGenerator` snapshot test covering the `Lextm.SharpSnmpLib.*` surface should be added to CI to catch regressions automatically.
