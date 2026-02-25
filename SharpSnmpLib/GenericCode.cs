using System.Runtime.Serialization;

namespace Lextm.SharpSnmpLib;

/// <summary>
/// Generic trap code.
/// </summary>
[DataContract]
public enum GenericCode
{
    ColdStart = 0,
    WarmStart = 1,
    LinkDown = 2,
    LinkUp = 3,
    AuthenticationFailure = 4,
    EgpNeighborLoss = 5,
    EnterpriseSpecific = 6
}
