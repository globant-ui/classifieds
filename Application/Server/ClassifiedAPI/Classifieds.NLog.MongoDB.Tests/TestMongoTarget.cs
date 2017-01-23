using System;
using System.Linq;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using NLog;

namespace Classifieds.NLog.MongoDB.Tests
{
    [TestFixture]
    public class TestMongoTarget
    {
        private Mock<IRepositoryProvider> _mockProvider;
        private Mock<IRepository> _mockRepository;
        private MongoServerSettings _settings;

        public void TestTarget()
        {
            _mockProvider = new Mock<IRepositoryProvider>();
            _mockRepository = new Mock<IRepository>();
            _settings = new MongoServerSettings();
        }

        [Test]
        public void TestSettingsAndRepository()
        {
            var databaseName = "Logging";
            var host = "mongodb://localhost";
            var port = 27017;

            _mockProvider.Setup(
                p => p.GetRepository(_settings, It.IsAny<string>()))
                .Returns(_mockRepository.Object)
                .Verifiable();

            var target = new MongoDBTarget
            {
                Database = databaseName,
                Host = host,
                Port = port,
                Provider = () => _mockProvider.Object
            };

            var eventLogInfo = new LogEventInfo();

            _mockRepository.Setup(
                r => r.Insert(eventLogInfo))
                .Verifiable();

            target.TestWrite(eventLogInfo);

            _mockProvider.Verify();
            _mockRepository.Verify();

            new MongoDBTarget().Host.Should().Be("mongodb://localhost");
            new MongoDBTarget().Port.Should().Be(27017);
            new MongoDBTarget().Database.Should().Be("Logging");
        }

        [Test]
        public void TestActualLog()
        {
            var logger = LogManager.GetLogger("MyTestClass");
            var server = new MongoServer(new MongoServerSettings
            {
                Server = new MongoServerAddress("mongodb://localhost", 27017)
            });
            var db = server.GetDatabase("Logging");
            var collection = db.GetCollection<LogEventInfoData>("logging");

            collection.RemoveAll();

            logger.LogException(
                LogLevel.Error, "Test Log Message",
                new Exception("Test Exception"));

            collection.FindAll().Count().Should().Be(1);

            var logEntry = collection.FindAll().First();

            logEntry.Level.Should().Be(LogLevel.Error.ToString());
            logEntry.Message.Should().Be("Test Log Message");
            logEntry.Exception.Message.Should().Be("Test Exception");


        }
    }
}
