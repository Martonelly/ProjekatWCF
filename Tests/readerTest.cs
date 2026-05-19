//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.IO;
using Client.Functions;

namespace Tests
{
    [TestFixture]
    public class readerTest
    {
        private ReadFromDataBase reader;

        [SetUp]
        public void Setup() {
            string path = Path.GetTempFileName();
            reader = new ReadFromDataBase(path);
        }

        [Test]
        public void TestAfterDispose() {
            //Disposing then tring to read line

            reader.Dispose();

            Action test = () => reader.FReadFromDataBase();

            Assert.Throws<ObjectDisposedException>(test);
        }
        [Test]

        public void TestDisposeAgain()
        {
            //Disposing one after another
            reader.Dispose();

            Action test = () => reader.Dispose();

            Assert.DoesNotThrow(test);
        }

    }
}
