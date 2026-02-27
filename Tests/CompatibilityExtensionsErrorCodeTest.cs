using DotNetSnmp.Asn1.SyntaxObjects;
using Lextm.SharpSnmpLib;
using Xunit;

namespace DotNetSnmp.Test;

public class CompatibilityExtensionsErrorCodeTest
{
    [Fact]
    public void ToErrorCode_KnownValue_ReturnsCode()
    {
        var raw = new Integer32((int)ErrorCode.TooBig);

        var code = raw.ToErrorCode();

        Assert.Equal(ErrorCode.TooBig, code);
    }

    [Fact]
    public void ToErrorCode_UnknownValue_Throws()
    {
        var raw = new Integer32(99);

        Assert.Throws<InvalidCastException>(() => raw.ToErrorCode());
    }

    [Fact]
    public void TryToErrorCode_KnownValue_ReturnsTrue()
    {
        var raw = new Integer32((int)ErrorCode.NoError);

        var ok = raw.TryToErrorCode(out var code);

        Assert.True(ok);
        Assert.Equal(ErrorCode.NoError, code);
    }

    [Fact]
    public void TryToErrorCode_UnknownValue_ReturnsFalse()
    {
        var raw = new Integer32(-1);

        var ok = raw.TryToErrorCode(out _);

        Assert.False(ok);
    }
}
