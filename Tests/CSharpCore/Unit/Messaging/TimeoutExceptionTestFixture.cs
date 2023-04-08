using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace Lextm.SharpSnmpLib.Messaging
{
    public class TimeoutExceptionTestFixture
    {
        [Fact]
        public void TimeoutException_DefaultConstructorTest()
        {
            // Arrange & Act
            var exception = new TimeoutException();

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public void TimeoutException_MessageConstructorTest()
        {
            // Arrange
            var testMessage = "Test message";

            // Act
            var exception = new TimeoutException(testMessage);

            // Assert
            Assert.Equal(testMessage, exception.Message);
        }

        [Fact]
        public void TimeoutException_MessageAndInnerExceptionConstructorTest()
        {
            // Arrange
            var testMessage = "Test message";
            var innerException = new InvalidOperationException();

            // Act
            var exception = new TimeoutException(testMessage, innerException);

            // Assert
            Assert.Equal(testMessage, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void TimeoutException_SerializationTest()
        {
            // Arrange
            var originalException = new TimeoutException("Test message", new InvalidOperationException());
            var binaryFormatter = new BinaryFormatter();

            // Act
            // Serialize the original exception to a memory stream
            using var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, originalException);
            memoryStream.Position = 0;

            // Deserialize the memory stream back into an object
            var deserializedException = (TimeoutException)binaryFormatter.Deserialize(memoryStream);

            // Assert
            Assert.NotNull(deserializedException);
            Assert.Equal(originalException.Message, deserializedException.Message);
            Assert.IsType<InvalidOperationException>(deserializedException.InnerException);
            Assert.Equal(originalException.Timeout, deserializedException.Timeout);
        }

        [Fact]
        public void TimeoutException_ToStringTest()
        {
            // Arrange
            var testTimeout = 1000;
            var exception = TimeoutException.Create(IPAddress.Loopback, testTimeout);

            // Act
            var result = exception.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.Contains(testTimeout.ToString(CultureInfo.InvariantCulture), result);
        }

        [Fact]
        public void TimeoutException_CreateTest()
        {
            // Arrange
            var testAgent = IPAddress.Loopback;
            var testTimeout = 1000;

            // Act
            var exception = TimeoutException.Create(testAgent, testTimeout);

            // Assert
            Assert.NotNull(exception);
            Assert.Equal(testAgent, exception.Agent);
            Assert.Equal(testTimeout, exception.Timeout);
        }

        [Fact]
        public void TimeoutException_Create_ThrowsArgumentNullException()
        {
            // Arrange
            IPAddress testAgent = null;
            var testTimeout = 1000;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => TimeoutException.Create(testAgent, testTimeout));
        }
    }
}
