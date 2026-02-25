using System.Runtime.Serialization;

namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Walk mode.
/// </summary>
[DataContract]
public enum WalkMode
{
    /// <summary>
    /// Default mode walk to the end of MIB view.
    /// </summary>
    Default = 0,

    /// <summary>
    /// In this mode, walk within sub-tree.
    /// </summary>
    WithinSubtree = 1
}
