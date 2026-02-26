# Quick Reference: Issues Needing area:faulty-device Label

## Issues to Tag

Run these commands with GitHub CLI (`gh`) or apply labels manually via GitHub UI:

```bash
# Issue #62 - HP server OctetString encoding
gh issue edit 62 --add-label "area:faulty-device"

# Issue #89 - MikroTik unsupported data type 18  
gh issue edit 89 --add-label "area:faulty-device"

# Issue #101 - Cisco ASA MsgFlags
gh issue edit 101 --add-label "area:faulty-device"

# Issue #370 - QNap NAS GetTable truncation
gh issue edit 370 --add-label "area:faulty-device"

# Issue #436 - Cisco switch MAC per VLAN
gh issue edit 436 --add-label "area:faulty-device"

# Issue #475 - Citrix Xen GetTable timeout
gh issue edit 475 --add-label "area:faulty-device"
```

## Or use the automated script:
```bash
./add-faulty-device-labels.sh
```

## Issue Links

Direct links to issues:
- [#62 - HP server OctetString](https://github.com/lextudio/sharpsnmplib/issues/62)
- [#89 - MikroTik data type](https://github.com/lextudio/sharpsnmplib/issues/89)
- [#101 - Cisco ASA MsgFlags](https://github.com/lextudio/sharpsnmplib/issues/101)
- [#370 - QNap GetTable](https://github.com/lextudio/sharpsnmplib/issues/370)
- [#436 - Cisco MAC per VLAN](https://github.com/lextudio/sharpsnmplib/issues/436)
- [#475 - Citrix GetTable](https://github.com/lextudio/sharpsnmplib/issues/475)

## Why These Issues?

All 6 issues meet the criteria for device-related problems:
- ✓ Specific device vendor/model with non-standard behavior
- ✓ Works in other SNMP tools (iReasoning, Net::SNMP, etc.)
- ✓ Device returns malformed or non-standard SNMP data
- ✓ Requires device-specific workaround

See `FAULTY_DEVICE_ISSUES.md` for detailed analysis.
