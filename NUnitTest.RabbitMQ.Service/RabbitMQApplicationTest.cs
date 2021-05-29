using NUnit.Framework;
using Wonga.Service.Consumer.Abstractions;
using Wonga.Service.Consumer.RabbitMQService;

namespace NUnitTest.RabbitMQ.Service
{
    public class RabbitMQApplicationTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ValidateMessage_MessageIsNull_ReturnFalse()
        {
            string message = string.Empty;
            var settings = new ApplicationSettings();
            var rabbitMqApplication = new RabbitMQApplication(settings);
            var result = rabbitMqApplication.ValidateMesssage(message);
            Assert.IsFalse(result);
        }
        [Test]
        public void ValidateMessage_MessageIsNotNull_ReturnTrue()
        {
            string message = "Hello my name is, Name";
            var settings = new ApplicationSettings();
            var rabbitMqApplication = new RabbitMQApplication(settings);
            var result = rabbitMqApplication.ValidateMesssage(message);
            Assert.IsTrue(result);
        }
        [Test]
        public void ValidateMessage_WrongMessageFormat_ReturnFalse()
        {
            string message = "Hello my name is Name";
            var settings = new ApplicationSettings();
            var rabbitMqApplication = new RabbitMQApplication(settings);
            var result = rabbitMqApplication.ValidateMesssage(message);
            Assert.IsFalse(result);
        }
    }
}