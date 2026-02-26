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
- **Issue**: OctetString field returning faulty hex values
- **Reason**: Device-specific data encoding issue

### #83 - Dell iDRAC 8 SNMPv3 Trap Issue
- **Status**: Needs tag
- **Device**: Dell iDRAC 8
- **Issue**: SNMPv3 trap receiving issue
- **Reason**: Works in PowerSNMP Manager but not SharpSnmpLib (device-specific compatibility)

### #89 - MikroTik RouterOS Unsupported Data Type
- **Status**: Needs tag
- **Device**: MikroTik RouterOS
- **Issue**: Unsupported data type 18 exception
- **Reason**: Device uses non-standard SNMP data types

### #101 - Cisco ASA 5510 MsgFlags Issue
- **Status**: Needs tag
- **Device**: Cisco ASA 5510 firewall
- **Issue**: Returns MsgFlags=8, library only handles 0-7
- **Reason**: Works in Net::SNMP, device returns out-of-spec value

### #114 - Fortis Device Data Construction Exception
- **Status**: Needs tag
- **Device**: Fortis device
- **Issue**: Data construction exception with mismatched communities
- **Reason**: Device-specific behavior

### #370 - QNap NAS GetTable Truncation
- **Status**: Needs tag
- **Device**: QNap NAS (Firmware 3.2.3 Build 0209T)
- **Issue**: GetTable returns truncated/misaligned table data
- **Reason**: Works in iReasoning Browser but not SharpSnmpLib (device-specific compatibility)

### #420 - HP v1910-24G Switch VLAN Query
- **Status**: Needs tag
- **Device**: HP v1910-24G switch
- **Issue**: VLAN query issue
- **Reason**: Device-specific behavior

### #436 - Cisco Switch MAC Address per VLAN
- **Status**: Needs tag
- **Device**: Cisco switch
- **Issue**: MAC address retrieval per VLAN in SNMPv3 returns empty data
- **Reason**: Device-specific VLAN context handling required

### #469 - HP OpenView Integration Issue
- **Status**: Needs tag
- **Device**: HP OpenView
- **Issue**: Integration issue
- **Reason**: Device/platform-specific compatibility

### #471 - Dell Server MIBs RFC1212 Loading
- **Status**: Needs tag
- **Device**: Dell server
- **Issue**: RFC1212 loading issue with Dell MIBs
- **Reason**: Device-specific MIB format

### #475 - Citrix Xen GetTable Timeout
- **Status**: Needs tag
- **Device**: Citrix Xen SNMP Agent
- **Issue**: GetTable operations timeout
- **Reason**: Works in iReasoning Browser but not SharpSnmpLib, single OID reads work (device-specific compatibility)

### #630 - HP Switch Firmware Download Status
- **Status**: Needs tag
- **Device**: HP switch
- **Issue**: Firmware download status field issue
- **Reason**: Device-specific behavior

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
- **Issues that need area:faulty-device tag**: 12
- **Total device-related issues**: 19

## Device Vendor Distribution

- **Cisco**: 3 issues (#101, #318, #436)
- **HP**: 4 issues (#62, #420, #469, #630)
- **Dell**: 2 issues (#83, #471)
- **QNap**: 1 issue (#370)
- **Citrix**: 1 issue (#475)
- **MikroTik**: 1 issue (#89)
- **Fortis**: 1 issue (#114)
- **Eaton**: 1 issue (#698)
- **Unknown/Generic**: 5 issues (#291, #474, #608, #693, #700)

## Recommendations

1. **Add the `area:faulty-device` label** to all 12 issues listed in the "Issues Requiring Label" section
2. **Maintain this label** for device-specific compatibility issues to help:
   - Distinguish between library bugs and device non-compliance
   - Track which devices have known issues
   - Prioritize compatibility improvements
   - Document workarounds for specific devices

## Notes

- Issues were reviewed based on their descriptions, comments, and whether they mention specific devices or work in other SNMP tools
- Some older issues may lack sufficient detail to make a definitive determination
- Device-specific issues often require workarounds rather than fixes, as the device behavior cannot be changed
