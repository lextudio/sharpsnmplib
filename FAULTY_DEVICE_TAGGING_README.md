# Faulty Device Tagging Task - README

## Task Overview

This task involved reviewing all imported issues (490 total) in the lextudio/sharpsnmplib repository to identify which ones are related to faulty or non-standard devices, and tagging them with the `area:faulty-device` label.

## What Was Done

### 1. Comprehensive Review
- Reviewed all 490 issues with the "imported" label
- Applied systematic criteria to identify device-related issues
- Verified each candidate issue by reading full descriptions and comments

### 2. Identification Criteria

Issues were identified as device-related if they met one or more of these criteria:

1. **Specific device vendor/model** exhibiting non-standard SNMP behavior
2. **Device returns malformed/truncated/incorrect** SNMP data  
3. **Works in other SNMP tools** but not in SharpSnmpLib (indicates device-specific compatibility)
4. **Explicitly mentions device bug** or firmware issue
5. **Requires device-specific workaround** rather than a general fix

### 3. Results

**Found:**
- 7 issues already properly tagged with `area:faulty-device`
- 6 issues needing the `area:faulty-device` label

**Issues requiring `area:faulty-device` label:**
- #62: HP server - OctetString encoding mismatch (works in iReasoning)
- #89: MikroTik RouterOS - unsupported data type 18
- #101: Cisco ASA 5510 - MsgFlags=8 out of range (works in Net::SNMP)
- #370: QNap NAS - GetTable truncation (works in iReasoning)
- #436: Cisco switch - MAC address per VLAN (needs VLAN context)
- #475: Citrix Xen - GetTable timeout (works in iReasoning)

## Files Created

### 1. `FAULTY_DEVICE_ISSUES.md`
Comprehensive documentation of:
- All device-related issues (both tagged and untagged)
- Detailed analysis of each issue
- Device vendor distribution
- Recommendations for labeling

### 2. `add-faulty-device-labels.sh`
Automated script to add labels using GitHub CLI:
```bash
./add-faulty-device-labels.sh
```

This script will:
- Add `area:faulty-device` label to the 6 identified issues
- Add `imported` label to issue #101
- Provide progress feedback

## How to Apply the Labels

### Option 1: Using the Script (Recommended)
```bash
# Ensure you have GitHub CLI installed and authenticated
gh auth status

# Run the script
./add-faulty-device-labels.sh
```

### Option 2: Manual GitHub UI
Navigate to each issue and add the `area:faulty-device` label:
- Issue #62
- Issue #89  
- Issue #101 (also add "imported")
- Issue #370
- Issue #436
- Issue #475

### Option 3: Using GitHub CLI Manually
```bash
gh issue edit 62 --add-label "area:faulty-device"
gh issue edit 89 --add-label "area:faulty-device"
gh issue edit 101 --add-label "area:faulty-device"
gh issue edit 370 --add-label "area:faulty-device"
gh issue edit 436 --add-label "area:faulty-device"
gh issue edit 475 --add-label "area:faulty-device"
```

## Verification

After applying labels, you can verify by running:
```bash
gh issue list --label "area:faulty-device" --label "imported" --state all
```

This should show 13 total issues (7 previously tagged + 6 newly tagged).

## Notes

- The `area:faulty-device` label (not just `faulty-device`) is used to match existing conventions in the repository
- Some issues that mentioned devices were determined NOT to be device bugs (usage questions, documentation requests, etc.) and were excluded
- The analysis focused on issues where the device behavior is non-standard or incompatible with SNMP specifications
- Issues that work in other SNMP tools but not in SharpSnmpLib are strong indicators of device-specific compatibility issues

## Device Breakdown

After tagging, the repository will have device-related issues from:
- **Cisco**: 3 issues (ASA, switches)
- **HP**: 1 issue (server)
- **QNap**: 1 issue (NAS)
- **Citrix**: 1 issue (Xen)
- **MikroTik**: 1 issue (RouterOS)
- **Eaton**: 1 issue (PDU)
- **Generic/Unknown devices**: 5 issues

Total: 13 device-related issues identified and documented.
