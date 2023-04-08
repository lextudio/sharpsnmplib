using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace Lextm.SharpSnmpLib.Messaging
{
    public class ErrorExceptionTestFixture
    {
        [Fact]
        public void ErrorException_DefaultConstructorTest()
        {
            // Arrange & Act
            var exception = new ErrorException();

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public void ErrorException_MessageConstructorTest()
        {
            // Arrange
            var testMessage = "Test message";

            // Act
            var exception = new ErrorException(testMessage);

            // Assert
            Assert.Equal(testMessage, exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void ErrorException_MessageAndInnerExceptionConstructorTest()
        {
            // Arrange
            var testMessage = "Test message";
            var innerException = new InvalidOperationException();

            // Act
            var exception = new ErrorException(testMessage, innerException);

            // Assert
            Assert.Equal(testMessage, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void ErrorException_SerializationTest()
        {
            // Arrange
            var originalException = new ErrorException("Test message", new InvalidOperationException());
            var binaryFormatter = new BinaryFormatter();

            // Act
            // Serialize the original exception to a memory stream
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, originalException);
                memoryStream.Position = 0;

                // Deserialize the memory stream back into an object
                var deserializedException = (ErrorException)binaryFormatter.Deserialize(memoryStream);

                // Assert
                Assert.NotNull(deserializedException);
                Assert.Equal(originalException.Message, deserializedException.Message);
                Assert.IsType<InvalidOperationException>(deserializedException.InnerException);
            }
        }

        [Fact]
        public void ErrorException_CreateTest()
        {
            // Arrange
            var testMessage = "Test message";
            var agent = IPAddress.Parse("127.0.0.1");
            var body = new ResponseMessage(0, VersionCode.V1, new OctetString("public"), ErrorCode.TooBig, 0, new Variable[0]);

            // Act
            var exception = ErrorException.Create(testMessage, agent, body);

            // Assert
            Assert.NotNull(exception);
            Assert.Equal(testMessage, exception.Message);
            Assert.Equal(agent, exception.Agent);
            Assert.Equal(body, exception.Body);
        }
    }
}
