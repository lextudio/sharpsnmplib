using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace Lextm.SharpSnmpLib.Messaging
{
    public class MessageFactoryExceptionTestFixture
    {
        [Fact]
        public void MessageFactoryException_DefaultConstructorTest()
        {
            // Arrange & Act
            var exception = new MessageFactoryException();

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public void MessageFactoryException_MessageConstructorTest()
        {
            // Arrange
            var testMessage = "Test message";

            // Act
            var exception = new MessageFactoryException(testMessage);

            // Assert
            Assert.Equal(testMessage, exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void MessageFactoryException_MessageAndInnerExceptionConstructorTest()
        {
            // Arrange
            var testMessage = "Test message";
            var innerException = new InvalidOperationException();

            // Act
            var exception = new MessageFactoryException(testMessage, innerException);

            // Assert
            Assert.Equal(testMessage, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void MessageFactoryException_SerializationTest()
        {
            // Arrange
            var originalException = new MessageFactoryException("Test message", new InvalidOperationException());
            originalException.SetBytes(new byte[] { 1, 2, 3, 4, 5 });
            var binaryFormatter = new BinaryFormatter();

            // Act
            // Serialize the original exception to a memory stream
            using var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, originalException);
            memoryStream.Position = 0;

            // Deserialize the memory stream back into an object
            var deserializedException = (MessageFactoryException)binaryFormatter.Deserialize(memoryStream);

            // Assert
            Assert.NotNull(deserializedException);
            Assert.Equal(originalException.Message, deserializedException.Message);
            Assert.IsType<InvalidOperationException>(deserializedException.InnerException);
            Assert.Equal(originalException.GetBytes(), deserializedException.GetBytes());
        }

        [Fact]
        public void MessageFactoryException_GetAndSetBytesTest()
        {
            // Arrange
            var testBytes = new byte[] { 1, 2, 3, 4, 5 };
            var exception = new MessageFactoryException();

            // Act
            exception.SetBytes(testBytes);
            var result = exception.GetBytes();

            // Assert
            Assert.Equal(testBytes, result);
        }

        [Fact]
        public void MessageFactoryException_ToStringTest()
        {
            // Arrange
            var testMessage = "Test message";
            var exception = new MessageFactoryException(testMessage);

            // Act
            var result = exception.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.Contains(testMessage, result);
        }
    }
}
