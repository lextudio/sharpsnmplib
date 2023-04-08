using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace Lextm.SharpSnmpLib.Messaging
{
    public class PortInUseExceptionTests
    {
        [Fact]
        public void PortInUseException_DefaultConstructorTest()
        {
            // Arrange & Act
            var exception = new PortInUseException();

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public void PortInUseException_MessageConstructorTest()
        {
            // Arrange
            var testMessage = "Test message";

            // Act
            var exception = new PortInUseException(testMessage);

            // Assert
            Assert.Equal(testMessage, exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void PortInUseException_MessageAndInnerExceptionConstructorTest()
        {
            // Arrange
            var testMessage = "Test message";
            var innerException = new InvalidOperationException();

            // Act
            var exception = new PortInUseException(testMessage, innerException);

            // Assert
            Assert.Equal(testMessage, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void PortInUseException_SerializationTest()
        {
            // Arrange
            var originalException = new PortInUseException("Test message", new InvalidOperationException());
            originalException.Endpoint = new IPEndPoint(IPAddress.Loopback, 161);
            var binaryFormatter = new BinaryFormatter();

            // Act
            // Serialize the original exception to a memory stream
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, originalException);
                memoryStream.Position = 0;

                // Deserialize the memory stream back into an object
                var deserializedException = (PortInUseException)binaryFormatter.Deserialize(memoryStream);

                // Assert
                Assert.NotNull(deserializedException);
                Assert.Equal(originalException.Message, deserializedException.Message);
                Assert.IsType<InvalidOperationException>(deserializedException.InnerException);
                Assert.Equal(originalException.Endpoint, deserializedException.Endpoint);
            }
        }

        [Fact]
        public void PortInUseException_ToStringTest()
        {
            // Arrange
            var testMessage = "Test message";
            var exception = new PortInUseException(testMessage);

            // Act
            var result = exception.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.Contains(testMessage, result);
        }
    }
}
