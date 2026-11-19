# Security Policy

## Supported Versions

We provide security updates for each version until its End-of-Life (EOL) date. Below is a summary of currently supported versions:

| Version | Latest Release | End of Life | Notes                     |
| ------- | -------------- | ----------- | ------------------------- |
| 13.0    | 13.0.0-beta.3  | TBD         | Preview, new design       |
| 12.5    | 12.5.7         | TBD         | EOL to be determined      |
| < 12.5  | -              | Reached EOL | Upgrade to 12.5.x         |

## Version Support Guidelines

To keep your C# SNMP (Lextm.SharpSnmpLib) installation secure and stable, follow these upgrade recommendations:

- **Always Upgrade to the Latest Patch Release**
  Patch releases fix security vulnerabilities and bugs. Use the latest patch version within your current major or minor release.
  *Example:* If you are on 12.5.6, upgrade to the latest 12.5.x.

- **Upgrade to the Next Minor Release Within 6 Months**
  Upgrade to the latest minor version within 6 months after a new minor release to keep receiving security updates.

- **Upgrade to the Next Major Release Within 1 Year**
  Major releases may introduce breaking changes. Upgrade to the latest major version within 1 year after it is released to keep receiving support and security updates.

## Reporting a Vulnerability

**Please do not report security vulnerabilities through public GitHub issues, discussions, or pull requests.**

Report them privately using either channel:

- GitHub private vulnerability reporting: open the **Security** tab of this repository and click **Report a vulnerability**
  (<https://github.com/lextudio/sharpsnmplib/security/advisories/new>).
- Email <support@lextudio.com> with "Security" in the subject line, if you cannot use GitHub private vulnerability reporting.

Please include:

- The affected package version(s) and target framework.
- A description of the issue and its impact.
- Steps or a minimal proof of concept to reproduce it.
- Any suggested fix or mitigation, if known.

We will acknowledge your report, investigate it, and coordinate a fix and disclosure timeline with you. Once a fix is released, we will publish a GitHub Security Advisory and credit the reporter unless they prefer to remain anonymous.
