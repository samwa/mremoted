using System;

using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;

namespace ActiveMQ.Examples
{
	public class TopicPublisher : IDisposable
	{
		private bool disposed;
		private readonly ISession session;
		private readonly ITopic topic;
		
		public TopicPublisher (ISession session, string topicName)
		{
			this.session = session;
			DestinationName = topicName;
			topic = new ActiveMQTopic(DestinationName);
			Producer = session.CreateProducer(topic);
			
		}
		
		public IMessageProducer Producer { get; private set; }
		
		public string DestinationName { get; private set; }
		
		public void SendMessage(string message)
		{
			if (disposed) throw new ObjectDisposedException(GetType().Name);
			var textMessage = Producer.CreateTextMessage(message);
			Producer.Send(textMessage);
		}
		
		public void Dispose()
		{
			if (disposed) return;
			Producer.Close();
			Producer.Dispose();
			disposed = true;
		}				
	}
}

