using System.Security.Cryptography;

namespace Lextm.SharpSnmpLib.Security;

/// <summary>Generates salt bytes for SNMPv3 privacy (legacy compatibility).</summary>
public class SaltGenerator
{
    /// <summary>Initializes a new instance.</summary>
    public SaltGenerator() { }

    /// <summary>Gets 8 random salt bytes.</summary>
    public byte[] GetSaltBytes()
    {
        return RandomNumberGenerator.GetBytes(8);
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(SaltGenerator);
}
