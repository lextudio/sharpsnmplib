Simple SNMP operations can be easily translated to #SNMP function calls, so that you don't need to always refer to [our documentation](http://help.sharpsnmp.com).

## GET Operations
To send an SNMP v1 GET message to an SNMP agent located at 192.168.1.2 and query on OID 1.3.6.1.2.1.1.1.0,

```
var result = Messenger.Get(VersionCode.V1, 
                           new IPEndPoint(IPAddress.Parse("192.168.1.2"), 161), 
                           new OctetString("public"),
                           new List<Variable>{new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0"))},
                           60000);
```

This operation will time out if no reply is received after 60 seconds (1 minute), and throw an exception (`TimeoutException`). If any error occurs, an `ErrorException` can be caught. All #SNMP exceptions are derived from `SnmpException`. 

The result returned is a list that matches the list of `Variable` objects sent. The `Variable` in this list contains the value of the OID. All value classes implement from `ISnmpData` interface.

## SET Operations
To send an SNMP v1 SET message to an SNMP agent located at 192.168.1.2 and set the value of OID 1.3.6.1.2.1.1.6.0 to "Shanghai",

```
var result = Messenger.Set(VersionCode.V1, 
                           new IPEndPoint(IPAddress.Parse("192.168.1.2"), 161), 
                           new OctetString("public"),
                           new List<Variable>{new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.6.0"), new OctetString("Shanghai"))},
                           60000);
```

## GET-NEXT Operations

To send an SNMP v1 GET-NEXT message to an SNMP agent located at 192.168.1.2 and query on OID 1.3.6.1.2.1.1.1.0,

```
GetNextRequestMessage message = new GetNextRequestMessage(0,
                              VersionCode.V1,
                              new OctetString("public"),
                              new List<Variable>{new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.6.0"))});
ISnmpMessage response = message.GetResponse(60000, new IPEndPoint(IPAddress.Parse("192.168.1.2"), 161));
if (response.Pdu().ErrorStatus.ToInt32() != 0) 
{
    throw ErrorException.Create(
        "error in response",
        receiver.Address,
        response);
}
var result = response.Pdu().Variables;
```

## GET-BULK Operations

To send an SNMP v2 GET-BULK message to an SNMP agent located at 192.168.1.2 and query on OID 1.3.6.1.2.1.1.1.0,

```
GetBulkRequestMessage message = new GetBulkRequestMessage(0,
                      VersionCode.V2,
                      new OctetString("public"),
                      0,
                      10,
                      new List<Variable>{new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.6.0"))});
ISnmpMessage response = message.GetResponse(60000, new IPEndPoint(IPAddress.Parse("192.168.1.2"), 161));
if (response.Pdu().ErrorStatus.ToInt32() != 0)
{
    throw ErrorException.Create(
        "error in response",
        receiver.Address,
        response);
}
var result = response.Pdu().Variables;
```

## Walk Operations

Walk is not an atomic operation. That means, it utilizes several GET-NEXT (SNMP v1 walk) or GET-BULK (v2 and above). The following code shows how to perform walk on an SNMP agent located at 192.168.1.2 starting at 1.3.6.1.2.1.1,

```
var result = new List<Variable>();
Messenger.Walk(VersionCode.V1, 
               new IPEndPoint(IPAddress.Parse("192.168.1.2"), 161), 
               new OctetString("public"), 
               new ObjectIdentifier("1.3.6.1.2.1.1"), 
               result, 
               60000, 
               WalkMode.WithinSubtree);
```

The result returned contains a list of all available OIDs (as `Variable`) in this SNMP agent that under tree node of 1.3.6.1.2.1.1.

\#SNMP supports two walk modes, `Default` and `WithinSubtree`.

`Messenger.Walk` is built GET-NEXT. Note that `Messenger.BulkWalk` should be used if the device supports SNMP v2, as it is built on GET-BULK and provide better performance.

```
var result = new List<Variable>();
Messenger.BulkWalk(VersionCode.V2, 
                   new IPEndPoint(IPAddress.Parse("192.168.1.2"), 161), 
                   new OctetString("public"), 
                   new ObjectIdentifier("1.3.6.1.2.1.1"), 
                   result, 
                   60000, 
                   10, 
                   WalkMode.WithinSubtree, 
                   null, 
                   null);
```

## TRAP Operations
It is usually an SNMP agent that sends out TRAP messages. The following code shows how to send an empty SNMP v1 TRAP message from 192.168.1.2 to an SNMP manager located at 192.168.1.3,

```
Messenger.SendTrapV1(new IPEndPoint(IPAddress.Parse("192.168.1.3"), 162), 
                     IPAddress.Parse("192.168.1.2"), 
                     new OctetString("public"), 
                     new ObjectIdentifier("1.3.6.1.2.1.1"), 
                     GenericCode.ColdStart, 
                     0, 
                     0, 
                     new List<Variable>();
```

SNMP v2 and above introduces a simplified TRAP v2 message,

```
Messenger.SendTrapV2(0, 
                     VersionCode.V2, 
                     new IPEndPoint(IPAddress.Parse("192.168.1.3"), 162), 
                     new OctetString("public"), 
                     new ObjectIdentifier("1.3.6.1.2.1.1"), 
                     0, 
                     new List<Variable>());
```

## INFORM Operations
It is usually an SNMP agent that sends out INFORM messages. The following code shows how to send an empty INFORM message to an SNMP manager located at 192.168.1.3,

```
Messenger.SendInform(0, 
                     VersionCode.V2, 
                     new IPEndPoint(IPAddress.Parse("192.168.1.3"), 162), 
                     new OctetString("public"), 
                     new ObjectIdentifier("1.3.6.1.2.1.1"), 
                     0, 
                     new List<Variable>(), 
                     2000, 
                     null, 
                     null);
```

The manager should send back a reply to this INFORM message. Otherwise, a `TimeoutException` occurs.

To help you understand how to use the API provided by #SNMP Library, there are more sample projects you can find under Samples folder in source code package. Both C# and VB.NET samples are available.

## SNMP v3 Operations
`Messenger` class does not support v3 operations, as they are more complicated. Thus, you need to construct v3 messages directly using classes that implement `ISnmpMessage` interface and send them out.

## Other Resources

* [Official Documentation](http://help.sharpsnmp.com)
* [Project on CodePlex](http://sharpsnmplib.codeplex.com)
* [Author's Blog](http://lextm.com)
* [Pro Edition](http://sharpsnmp.com)