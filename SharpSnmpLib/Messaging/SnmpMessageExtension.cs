namespace Lextm.SharpSnmpLib.Messaging;

/// <summary>
/// Backward-compatible entry point for legacy extension APIs.
/// </summary>
[Obsolete("This type is for internal use only and may be removed in a future release.")]
public static class SnmpMessageExtension
{
    /// <summary>
    /// Gets a value indicating whether current runtime is Windows.
    /// </summary>
    public static bool IsRunningOnWindows => OperatingSystem.IsWindows();

    /// <summary>
    /// Gets a value indicating whether current runtime is macOS.
    /// </summary>
    public static bool IsRunningOnMac => OperatingSystem.IsMacOS();

    /// <summary>
    /// Gets a value indicating whether current runtime is iOS.
    /// </summary>
    public static bool IsRunningOnIOS => OperatingSystem.IsIOS();

    /// <summary>
    /// Tests whether current runtime is Mono.
    /// </summary>
    public static bool IsRunningOnMono()
    {
        return Type.GetType("Mono.Runtime") != null;
    }
}
