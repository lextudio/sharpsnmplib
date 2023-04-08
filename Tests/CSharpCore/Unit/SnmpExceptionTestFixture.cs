using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace Lextm.SharpSnmpLib.Tests
{
    public class SnmpExceptionTests
    {
        [Fact]
        public void SnmpException_SerializationTest()
        {
            // Arrange
            var originalException = new SnmpException("Test message", new InvalidOperationException());
            var binaryFormatter = new BinaryFormatter();

            // Act
            // Serialize the original exception to a memory stream
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, originalException);
                memoryStream.Position = 0;

                // Deserialize the memory stream back into an object
                var deserializedException = (SnmpException)binaryFormatter.Deserialize(memoryStream);

                // Assert
                Assert.NotNull(deserializedException);
                Assert.Equal(originalException.Message, deserializedException.Message);
                Assert.IsType<InvalidOperationException>(deserializedException.InnerException);
            }
        }
    }
}
