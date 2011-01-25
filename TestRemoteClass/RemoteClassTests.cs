using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Samwa
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class RemoteClassTests
    {
        public RemoteClassTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Add2Clients()
        {
            RemoteClass rc = new RemoteClass();

            int clientId1 = rc.AddClient();
            int clientId2 = rc.AddClient();

            Assert.AreNotEqual(clientId1, clientId2);
        }

        [TestMethod]
        public void Add2ThenRemove1()
        {
            RemoteClass rc = new RemoteClass();

            int clientId1 = rc.AddClient();
            int clientId2 = rc.AddClient();

            bool removed = rc.CloseClient(clientId1);

            Assert.AreEqual(rc.Clients[0], clientId2);
        }

        [TestMethod]
        public void GetNextClient()
        {
            RemoteClass rc = new RemoteClass();

            int clientId1 = rc.AddClient();
            int clientId2 = rc.AddClient();

            if (rc.RequestLock(clientId1))
            {
                bool nextClientSet = rc.SetNextClient();

                int nextClient = rc.NextClient;
                rc.ReleaseLock(clientId1);
            }

            if (rc.RequestLock(clientId2))
            {
                bool nextClientSet = rc.SetNextClient();

                int nextClient = rc.NextClient;
                rc.ReleaseLock(clientId2);
            }
        }

        [TestMethod]
        public void GetPi()
        {
            RemoteClass rc = new RemoteClass();

            rc.Iterate();
            rc.Iterate();
            rc.Iterate();

            double pi = rc.Pi;
        }

        [TestMethod]
        public void AddSpecificClient()
        {
            RemoteClass rc = new RemoteClass();

            rc.AddClient(2);

            Assert.AreEqual(rc.Clients.First(), 2);

        }
    }
}
