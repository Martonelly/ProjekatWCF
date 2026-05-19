using Client.Functions;
using NUnit.Framework;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    internal class serverWriteTest
    {
        private SessionService service;
        [SetUp]
        public void Setup() {
            service = new SessionService();
        }
        
        [Test]
        public void DisopseAgain() {
            service.Dispose();

            Action test = () => service.Dispose();

            Assert.DoesNotThrow(test);
        }
    }
}
