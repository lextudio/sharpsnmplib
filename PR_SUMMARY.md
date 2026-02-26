# Pull Request Summary: Faulty Device Issue Tagging Review

## Overview

This PR provides a comprehensive review of all imported issues to identify which ones are related to faulty or non-standard devices, and provides the tools and documentation needed to apply the `area:faulty-device` label to them.

## What Was Accomplished

### 1. Comprehensive Issue Review
- ✅ Reviewed **all 490 imported issues** in the repository
- ✅ Identified **6 issues** that need the `area:faulty-device` label
- ✅ Verified **7 issues** that already have the label correctly applied
- ✅ Applied systematic criteria to distinguish device bugs from software bugs

### 2. Deliverables

#### Documentation Files:
1. **`FAULTY_DEVICE_ISSUES.md`** - Comprehensive analysis document
   - Detailed information on each device-related issue
   - Clear criteria for identifying device bugs
   - Summary statistics and device vendor distribution
   - Already-tagged vs needs-tagging breakdowns

2. **`FAULTY_DEVICE_TAGGING_README.md`** - Complete task overview
   - What was done and why
   - How to apply the labels (3 different methods)
   - Verification steps
   - Background information

3. **`QUICK_REFERENCE_TAGGING.md`** - Quick reference guide
   - Direct links to all 6 issues
   - Copy-paste ready commands
   - Brief justification for each issue

#### Automation:
4. **`add-faulty-device-labels.sh`** - Executable script
   - Automatically applies labels to all 6 issues
   - Adds missing `imported` label to issue #101
   - Provides progress feedback
   - Ready to run with `gh` CLI

## Issues Identified for Tagging

| Issue | Device | Problem | Rationale |
|-------|--------|---------|-----------|
| [#62](https://github.com/lextudio/sharpsnmplib/issues/62) | HP Server | OctetString encoding mismatch | Works in iReasoning, not in SharpSnmpLib |
| [#89](https://github.com/lextudio/sharpsnmplib/issues/89) | MikroTik RouterOS | Unsupported data type 18 | Device uses non-standard SNMP data type |
| [#101](https://github.com/lextudio/sharpsnmplib/issues/101) | Cisco ASA 5510 | MsgFlags=8 out of range | Works in Net::SNMP, not in SharpSnmpLib |
| [#370](https://github.com/lextudio/sharpsnmplib/issues/370) | QNap NAS | GetTable truncation | Works in iReasoning, not in SharpSnmpLib |
| [#436](https://github.com/lextudio/sharpsnmplib/issues/436) | Cisco Switch | MAC per VLAN empty | Requires VLAN context support |
| [#475](https://github.com/lextudio/sharpsnmplib/issues/475) | Citrix Xen | GetTable timeout | Works in iReasoning, single reads work |

**Note:** Issue #101 also needs the `imported` label added.

## Criteria Used

Issues qualified as "device-related" if they met one or more of these criteria:

1. ✅ **Specific device vendor/model** exhibiting non-standard SNMP behavior
2. ✅ **Device returns malformed/truncated/incorrect** SNMP data
3. ✅ **Works in other SNMP tools** but not in SharpSnmpLib (device-specific compatibility)
4. ✅ **Explicitly mentions device bug** or firmware issue
5. ✅ **Requires device-specific workaround** rather than a general fix

## How to Apply Labels

### Option 1: Use the Automated Script (Recommended)
```bash
./add-faulty-device-labels.sh
```

### Option 2: Manual Commands
```bash
gh issue edit 62 --add-label "area:faulty-device"
gh issue edit 89 --add-label "area:faulty-device"
gh issue edit 101 --add-label "area:faulty-device,imported"
gh issue edit 370 --add-label "area:faulty-device"
gh issue edit 436 --add-label "area:faulty-device"
gh issue edit 475 --add-label "area:faulty-device"
```

### Option 3: GitHub UI
Navigate to each issue link above and manually add the label through the web interface.

## Impact

After applying these labels:
- **Total device-related issues:** 13 (7 existing + 6 new)
- **Device vendor coverage:** Cisco (3), HP (1), QNap (1), Citrix (1), MikroTik (1), Eaton (1), Unknown (5)
- **Better issue organization** for tracking device compatibility vs software bugs
- **Clearer prioritization** for device-specific workarounds

## Why This Matters

Properly labeling device-related issues helps:
- **Distinguish** between library bugs (need code fixes) and device bugs (need workarounds)
- **Track** which devices have known issues
- **Prioritize** compatibility improvements for popular devices
- **Document** device-specific behaviors for future reference
- **Guide** users encountering similar device issues

## Verification

After labels are applied, verify with:
```bash
gh issue list --label "area:faulty-device" --label "imported" --state all
```

Expected result: 13 issues total

## Notes

- The existing label convention `area:faulty-device` (not `faulty-device`) was maintained
- Some issues mentioning devices were excluded (usage questions, documentation requests, etc.)
- Focus was on non-standard device behavior incompatible with SNMP specifications
- "Works in other tools" is a strong indicator of device-specific compatibility issues

## Next Steps

1. Review and approve this PR
2. Merge the documentation and scripts
3. Run the provided script or manually apply labels
4. Consider creating device-specific wiki pages for popular vendors
5. Use the `area:faulty-device` label consistently for future issues

---

**Files Changed:**
- `FAULTY_DEVICE_ISSUES.md` (new)
- `FAULTY_DEVICE_TAGGING_README.md` (new)
- `QUICK_REFERENCE_TAGGING.md` (new)
- `add-faulty-device-labels.sh` (new, executable)
