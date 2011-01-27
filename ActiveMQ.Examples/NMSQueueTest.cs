using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Apache.NMS.ActiveMQ;
using Apache.NMS;
using System.Threading;

namespace ActiveMQ.Examples
{
    [TestFixture]
    public class NMSQueueTest
    {
        private IConnection connection;
        private IConnectionFactory connectionFactory;
        private ISession session;

        private List<QueueReceiver> receivers = new List<QueueReceiver>();
        private QueueSender sender;

        const string QUEUE_NAME = "TestQueue";
        private const string BROKER = "tcp://localhost:61616";
        const string CLIENT_ID = "samwa.test.clientId";
        const string CONSUMER_ID = "samwa.test.subscriber";

        [SetUp]
        public void Setup()
        {
            connectionFactory = new ConnectionFactory(BROKER, CLIENT_ID);
            connection = connectionFactory.CreateConnection();
            connection.Start();
            session = connection.CreateSession();

            for (int i = 0; i < 100; i++)
            {
                var receiver = new QueueReceiver(session, QUEUE_NAME);
                receiver.OnMessageRecieved += (message => Console.WriteLine(message));
                receiver.Start(String.Format("Receiver#{0}", i));
                receivers.Add(receiver);
            }
        }

        [Test]
        public void TestSend()
        {
            sender = new QueueSender(session, QUEUE_NAME);
            for (int i = 0; i < 10; i++)
            {
                sender.SendMessage(String.Format("messagesend#{0}", i));
            }
        }

        [TearDown]
        public void Teardown()
        {
            Thread.Sleep(1000);
            sender.Dispose();
            receivers.ForEach(r => r.Dispose());
            session.Dispose();
            connection.Dispose();
        }
    }
}
