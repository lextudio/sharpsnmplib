// generate OperationExceptionTestFixture class that contains test cases for OperationException
//
// Path: Tests/CSharpCore/Unit/OperationExceptionTestFixture.cs
using System;
using System.Net;
using System.Runtime.Serialization;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit
{

    public class OperationExceptionTestFixture
    {
        [Fact]
        public void TestConstructor()
        {
            var ex = new OperationException();
            Assert.Null(ex.InnerException);
            Assert.Null(ex.Agent);
        }

        [Fact]
        public void TestConstructorWithMessage()
        {
            var ex = new OperationException("test");
            Assert.Equal("test", ex.Message);
            Assert.Null(ex.InnerException);
            Assert.Null(ex.Agent);
        }

        [Fact]
        public void TestConstructorWithMessageAndInnerException()
        {
            var inner = new Exception();
            var ex = new OperationException("test", inner);
            Assert.Equal("test", ex.Message);
            Assert.Equal(inner, ex.InnerException);
            Assert.Null(ex.Agent);
        }

        [Fact]
        public void TestConstructorWithSerializationInfoAndStreamingContext()
        {
            var info = new SerializationInfo(typeof(OperationException), new FormatterConverter());
            info.AddValue("Message", "Operation failed.");
            info.AddValue("InnerException", null);
            info.AddValue("HelpURL", null);
            info.AddValue("StackTraceString", null);
            info.AddValue("RemoteStackTraceString", null);
            info.AddValue("HResult", 0);
            info.AddValue("Source", null);
            info.AddValue("Agent", null);
            var context = new StreamingContext();
            var ex = new OperationException(info, context);
            Assert.Equal("Operation failed.", ex.Message);
            Assert.Null(ex.InnerException);
            Assert.Null(ex.Agent);
        }

        [Fact]
        public void TestGetObjectData()
        {
            var info = new SerializationInfo(typeof(OperationException), new FormatterConverter());
            var context = new StreamingContext();
            var ex = new OperationException("Operation failed.");
            ex.GetObjectData(info, context);
            Assert.Equal("Operation failed.", info.GetString("Message"));
            Assert.Null(info.GetString("Agent"));
        }

        [Fact]
        public void TestCreate()
        {
            var ex = OperationException.Create("test", IPAddress.Loopback);
            Assert.Equal("test", ex.Message);
            Assert.Null(ex.InnerException);
            Assert.Equal(IPAddress.Loopback, ex.Agent);
        }
    }
}
