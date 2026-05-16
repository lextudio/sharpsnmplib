using Lextm.SharpSnmpLib;
// removed: DotNetSnmp.Common.Definitions
// removed: DotNetSnmp.Protocol.V1
// removed: DotNetSnmp.Protocol.V3
using System.Text;
using Xunit;

namespace DotNetSnmp.Test;

public class OctetStringEncodingCompatibilityTest
{
    [Fact]
    public void DefaultEncoding_Applies_To_StringConstructor()
    {
        var original = OctetString.DefaultEncoding;
        try
        {
            OctetString.DefaultEncoding = Encoding.Unicode;

            var value = new OctetString("ctx");

            Assert.Equal(Encoding.Unicode.GetBytes("ctx"), value.Octets);
            Assert.Equal("ctx", value.ToString());
        }
        finally
        {
            OctetString.DefaultEncoding = original;
        }
    }

    [Fact]
    public void ExplicitEncoding_Overrides_DefaultEncoding()
    {
        var original = OctetString.DefaultEncoding;
        try
        {
            OctetString.DefaultEncoding = Encoding.ASCII;

            var value = new OctetString("é", Encoding.UTF8);

            Assert.Equal(Encoding.UTF8.GetBytes("é"), value.Octets);
            Assert.Equal("é", value.ToString());
            Assert.Equal("é", value.ToString(Encoding.UTF8));
        }
        finally
        {
            OctetString.DefaultEncoding = original;
        }
    }

    [Fact]
    public void ScopeCompatibility_Uses_DefaultEncoding_For_ContextName()
    {
        var original = OctetString.DefaultEncoding;
        try
        {
            OctetString.DefaultEncoding = Encoding.Unicode;

            var scope = new Scope(new OctetString(new byte[] { 1, 2, 3 }), new OctetString("ctx"), new GetRequestPdu());
            IScope legacyScope = scope;

            Assert.Equal("ctx", legacyScope.ContextName.ToString());
            Assert.Equal(Encoding.Unicode.GetBytes("ctx"), legacyScope.ContextName.Octets);
        }
        finally
        {
            OctetString.DefaultEncoding = original;
        }
    }
}
