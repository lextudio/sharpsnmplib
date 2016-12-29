using System;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// .NET standard 1.3 only helper.
    /// </summary>
    public interface ITypeResolver
    {
        Type Load(string assembly, string name);
    }
}
