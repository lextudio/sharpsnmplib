namespace Lextm.SharpSnmpLib.Security;

/// <summary>Extension methods for IPrivacyProvider (legacy compatibility).</summary>
public static class PrivacyProviderExtension
{
    /// <summary>Gets the security level for a privacy provider.</summary>
    public static Levels ToSecurityLevel(this IPrivacyProvider provider)
    {
        if (provider == null) throw new ArgumentNullException(nameof(provider));
        if (provider is DefaultPrivacyProvider def && def.AuthenticationProvider is DefaultAuthenticationProvider)
            return (Levels)0;
        if (provider is DefaultPrivacyProvider)
            return Levels.Authentication;
        return Levels.Authentication | Levels.Privacy;
    }

    /// <summary>Gets scope data (legacy compatibility stub).</summary>
    public static ISnmpData GetScopeData(this IPrivacyProvider provider, Header header, SecurityParameters parameters, ISnmpData scopeData)
        => scopeData;

    /// <summary>Computes hash (legacy compatibility stub).</summary>
    public static void ComputeHash(this IPrivacyProvider provider, VersionCode version, Header header, SecurityParameters parameters, Scope scope)
    {
        // Legacy stub — hash computation is handled internally in v13
    }

    /// <summary>Verifies hash (legacy compatibility stub).</summary>
    public static bool VerifyHash(this IPrivacyProvider provider, VersionCode version, Header header, SecurityParameters parameters, ISnmpData scopeData, byte[] wholePacket)
        => true;
}
