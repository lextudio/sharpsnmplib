using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Xunit;

namespace DotNetSnmp.Test;

public sealed class MessengerStaticCompatibilityTest
{
    [Fact]
    public void CounterPropertiesReturnAdvancingIds()
    {
        var request1 = Messenger.NextRequestId;
        var request2 = Messenger.NextRequestId;
        var message1 = Messenger.NextMessageId;
        var message2 = Messenger.NextMessageId;

        Assert.NotEqual(request1, request2);
        Assert.NotEqual(message1, message2);
    }

    [Fact]
    public void MaxMessageSizeCanBeChanged()
    {
        var original = Messenger.MaxMessageSize;
        try
        {
            Messenger.MaxMessageSize = 2048;
            Assert.Equal(2048, Messenger.MaxMessageSize);
        }
        finally
        {
            Messenger.MaxMessageSize = original;
        }
    }

    [Fact]
    public void SecurityErrorOidsAndMessagesMatchLegacyMappings()
    {
        Assert.Equal("1.3.6.1.6.3.15.1.1.1.0", Messenger.UnsupportedSecurityLevel.ToString());
        Assert.Equal("1.3.6.1.6.3.15.1.1.2.0", Messenger.NotInTimeWindow.ToString());
        Assert.Equal("1.3.6.1.6.3.15.1.1.3.0", Messenger.UnknownSecurityName.ToString());
        Assert.Equal("1.3.6.1.6.3.15.1.1.4.0", Messenger.UnknownEngineId.ToString());
        Assert.Equal("1.3.6.1.6.3.15.1.1.5.0", Messenger.AuthenticationFailure.ToString());
        Assert.Equal("1.3.6.1.6.3.15.1.1.6.0", Messenger.DecryptionError.ToString());

        Assert.Equal("unsupported security level", Messenger.UnsupportedSecurityLevel.GetErrorMessage());
        Assert.Equal("not in time window", Messenger.NotInTimeWindow.GetErrorMessage());
        Assert.Equal("unknown security name", Messenger.UnknownSecurityName.GetErrorMessage());
        Assert.Equal("unknown engine ID", Messenger.UnknownEngineId.GetErrorMessage());
        Assert.Equal("authentication failure", Messenger.AuthenticationFailure.GetErrorMessage());
        Assert.Equal("decryption error", Messenger.DecryptionError.GetErrorMessage());
        Assert.Equal("unknown error", new ObjectIdentifier("1.3.6.1.4.1.0").GetErrorMessage());
    }
}
