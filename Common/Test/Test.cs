using Common.Contracts;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;

namespace Common.Test
{
    public class SolarClientTests
    {
        [Test]
        public void TransferData_WhenServiceIsDown_LogsErrorAndAborts()
        {
            // 1. ARRANGE
            var mockService = new Mock<ISessionService>();
            var mockLogger = new Mock<ILogger>();

            // This is the "Magic": Force an EndpointNotFoundException (Server down)
            mockService
                .Setup(s => s.StartSession(It.IsAny<PvMeta>()))
                .Throws(new EndpointNotFoundException("Target server not found."));

            // We also need to mock the IClientChannel behavior for the Abort() call
            var mockChannel = mockService.As<IClientChannel>();

            var processor = new SolarDataProcessor(mockService.Object, mockLogger.Object);
            var dummyData = new List<PvSample>();
            var meta = new PvMeta("test.csv", 0, "1.0", 0);

            // 2. ACT & ASSERT
            // We expect the processor to throw the exception back up
            Assert.Throws<EndpointNotFoundException>((NUnit.Framework.TestDelegate)(() =>
                processor.TransferData(dummyData, meta)));

            // VERIFY: Did it actually log the error?
            mockLogger.Verify(l => l.Error(It.Is<string>(s => s.Contains("Target server not found"))), Times.Once);

            // VERIFY: Did it call Abort on the channel?
            mockChannel.Verify(c => c.Abort(), Times.Once);
        }
    }
}