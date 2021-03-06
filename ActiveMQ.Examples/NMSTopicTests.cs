using System;
using NUnit.Framework;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System.Threading;
namespace ActiveMQ.Examples
{
	[TestFixture]
	public class NMSTopicTests
	{
		private IConnection connection;
		private IConnectionFactory connectionFactory;
		private TopicSubscriber subscriber;
		private ISession session;
		
		const string TOPIC_NAME = "TestTopic";
		private const string BROKER = " tcp://localhost:61616";
		const string CLIENT_ID = "samwa.test.clientId";
		const string CONSUMER_ID = "samwa.test.subscriber";
		
		[SetUp]
		public void SetUp()
		{
			connectionFactory = new ConnectionFactory(BROKER, CLIENT_ID);
			connection = connectionFactory.CreateConnection();
			connection.Start();
			session = connection.CreateSession();
			
			subscriber = new TopicSubscriber(session, TOPIC_NAME);
			subscriber.Start(CONSUMER_ID);
			subscriber.OnMessageReceived += 
				message => Console.WriteLine("Recieving message=>{0}", message);
		}

        [Test]
        public void Publish()
        {
            using (var publisher = new TopicPublisher(session, TOPIC_NAME))
            {
                publisher.SendMessage("test message");
            }
        }

        [TearDown]
        public void Teardown()
        {
            Thread.Sleep(1000);
            subscriber.Dispose();
            session.Close();
            session.Dispose();

            connection.Stop();
            connection.Close();
            connection.Dispose();
        }
	}
}

