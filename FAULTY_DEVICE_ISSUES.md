# Faulty Device Issues Review

This document lists all imported issues that are related to faulty or non-standard devices and should be tagged with the `area:faulty-device` label.

## Review Date
2026-02-26

## Criteria for Faulty Device Issues

Issues are classified as "faulty device" related if they meet one or more of the following criteria:

1. **Specific device vendor/model** exhibiting non-standard SNMP behavior
2. **Device returns malformed/truncated/incorrect** SNMP data
3. **Works in other SNMP tools** but not in SharpSnmpLib (indicates device-specific compatibility issue)
4. **Explicitly mentions device bug** or firmware issue
5. **Requires device-specific workaround** rather than a general fix

## Issues Requiring `area:faulty-device` Label

The following imported issues are related to faulty devices but currently **missing** the `area:faulty-device` label:

### #62 - HP Server OctetString Issue
- **Status**: Needs tag
- **Device**: HP server (OID .1.3.6.1.4.1.232.11.2.10.7.0)
- **Issue**: OctetString field returning different hex values than iReasoning Browser
- **Reason**: Works correctly in iReasoning Browser but returns "completely different" values in SharpSnmpLib - device-specific data encoding issue

### #89 - MikroTik RouterOS Unsupported Data Type
- **Status**: Needs tag
- **Device**: MikroTik RouterOS
- **Issue**: BulkWalk causes "unsupported data type: 18" exception (SNMPv2), but Walk with SNMPv1 works fine
- **Reason**: Device uses non-standard SNMP data type 18 that is not part of standard SNMP data types

### #101 - Cisco ASA 5510 MsgFlags Issue
- **Status**: Needs tag (note: currently only has "bug" label, missing "imported" label too)
- **Device**: Cisco ASA 5510 firewall
- **Issue**: Returns MsgFlags=8 during SNMPv3 discovery, library only handles 0-7
- **Reason**: Works in Net::SNMP, device returns out-of-spec value; Cisco C2960 switch works correctly

### #370 - QNap NAS GetTable Truncation
- **Status**: Needs tag
- **Device**: QNap NAS (Firmware 3.2.3 Build 0209T)
- **Issue**: GetTable on hrStorageTable returns only 6 columns instead of 7, with misaligned/shifted values
- **Reason**: Works correctly in iReasoning Browser but fails in SharpSnmpLib (device-specific compatibility)

### #436 - Cisco Switch MAC Address per VLAN
- **Status**: Needs tag
- **Device**: Cisco switch
- **Issue**: MAC address retrieval per VLAN in SNMPv3 returns empty data (OID 1.3.6.1.2.1.17.4.3.1.1)
- **Reason**: Requires VLAN context support (community@VLANnum in v2c) which may not be properly supported in v3

### #475 - Citrix Xen GetTable Timeout
- **Status**: Needs tag
- **Device**: Citrix Xen SNMP Agent
- **Issue**: GetTable operations timeout on sessionTable and procTable
- **Reason**: Works in iReasoning Browser, single OID reads work fine with SharpSnmpLib (device-specific GetTable compatibility)

## Issues Already Properly Tagged

The following issues already have the `area:faulty-device` label:

- **#291** - System.ArgumentException truncation error for 32-bit integer coding
- **#318** - Small Bug with Cisco SNMP Trap Messages (Timeticks encoding)
- **#474** - Constraining Request ID values (device truncates to 2 bytes)
- **#608** - Trap listener error (malformed trap messages from device)
- **#693** - Integer32 Truncation Exception (non-minimal BER encoding)
- **#698** - Misleading exception with Eaton PDUs (incorrect username in Report-PDU)
- **#700** - SNMP GetResponse with non-minimal INTEGER request-id

## Summary Statistics

- **Total imported issues reviewed**: 490
- **Issues already tagged with area:faulty-device**: 7
- **Issues that need area:faulty-device tag**: 6
- **Total device-related issues**: 13

## Device Vendor Distribution

- **Cisco**: 3 issues (#101, #318, #436)
- **HP**: 1 issue (#62)
- **Dell**: 0 confirmed device issues
- **QNap**: 1 issue (#370)
- **Citrix**: 1 issue (#475)
- **MikroTik**: 1 issue (#89)
- **Eaton**: 1 issue (#698)
- **Unknown/Generic**: 5 issues (#291, #474, #608, #693, #700)

## Recommendations

1. **Add the `area:faulty-device` label** to the 6 issues listed in the "Issues Requiring Label" section:
   - #62 (HP server OctetString)
   - #89 (MikroTik unsupported data type)
   - #101 (Cisco ASA MsgFlags) - also needs "imported" label
   - #370 (QNap GetTable)
   - #436 (Cisco MAC address per VLAN)
   - #475 (Citrix GetTable timeout)

2. **Maintain this label** for device-specific compatibility issues to help:
   - Distinguish between library bugs and device non-compliance
   - Track which devices have known issues
   - Prioritize compatibility improvements
   - Document workarounds for specific devices

3. **Consider creating** device-specific documentation or wiki pages for known compatibility issues with popular devices (Cisco, HP, Dell, etc.)

## Notes

- Issues were reviewed based on their descriptions, comments, and whether they mention specific devices or work in other SNMP tools
- Some older issues may lack sufficient detail to make a definitive determination
- Device-specific issues often require workarounds rather than fixes, as the device behavior cannot be changed
