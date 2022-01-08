using System;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit
{
    public class ObjectIdentifierConverterTestFixture
    {
        [Fact]
        public void CanConvertFromTest()
        {
            var converter = new ObjectIdentifierConverter();
            Assert.True(converter.CanConvertFrom(typeof(string)));
            Assert.False(converter.CanConvertFrom(typeof(DateTime)));
        }

        [Fact]
        public void CanConvertToTest()
        {
            var converter = new ObjectIdentifierConverter();
            Assert.True(converter.CanConvertTo(typeof(string)));
            Assert.False(converter.CanConvertTo(typeof(DateTime)));
        }

        [Fact]
        public void ConvertFromTest()
        {
            var converter = new ObjectIdentifierConverter();
            Assert.Equal(new ObjectIdentifier("0.0"), converter.ConvertFrom("0.0"));

            Assert.Throws<NotSupportedException>(() => converter.ConvertFrom((string)null));
            Assert.Throws<NotSupportedException>(() => converter.ConvertFrom("notgoodstring"));
        }

        [Fact]
        public void ConvertToTest()
        {
            var converter = new ObjectIdentifierConverter();
            Assert.Equal("0.0", converter.ConvertTo(new ObjectIdentifier("0.0"), typeof(string)));

            Assert.Equal(string.Empty, converter.ConvertTo((ObjectIdentifier)null, typeof(string)));
        }

        [Fact]
        public void ToStringTest()
        {
            var converter = new ObjectIdentifierConverter();
            Assert.Equal("Object identifier converter", converter.ToString());
        }
    }
}
