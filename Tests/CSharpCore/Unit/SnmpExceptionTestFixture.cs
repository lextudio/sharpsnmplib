using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace Lextm.SharpSnmpLib.Tests
{
    public class SnmpExceptionTestFixture
    {
        [Fact]
        public void SnmpException_DefaultConstructorTest()
        {
            // Arrange & Act
            var exception = new SnmpException();

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public void SnmpException_MessageConstructorTest()
        {
            // Arrange
            var testMessage = "Test message";

            // Act
            var exception = new SnmpException(testMessage);

            // Assert
            Assert.Equal(testMessage, exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void SnmpException_MessageAndInnerExceptionConstructorTest()
        {
            // Arrange
            var testMessage = "Test message";
            var innerException = new InvalidOperationException();

            // Act
            var exception = new SnmpException(testMessage, innerException);

            // Assert
            Assert.Equal(testMessage, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void SnmpException_ToStringTest()
        {
            // Arrange
            var testMessage = "Test message";
            var innerException = new InvalidOperationException("Inner exception message");
            var exception = new SnmpException(testMessage, innerException);

            // Act
            var result = exception.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.Contains(testMessage, result);
            Assert.Contains(innerException.ToString(), result);
        }
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
