using System;
using Apache.NMS;
using Apache.NMS.Util;

namespace Samwa.Masters
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// Example connection strings:
	        //    activemq:tcp://activemqhost:61616
	        //    stomp:tcp://activemqhost:61613
	        //    ems:tcp://tibcohost:7222
	        //    msmq://localhost
	
	        Uri connecturi = new Uri("tcp://127.0.0.1:61616");
	        
	        Console.WriteLine("About to connect to " + connecturi);
	
	        // NOTE: ensure the nmsprovider-activemq.config file exists in the executable folder.
	        IConnectionFactory factory = new NMSConnectionFactory(connecturi);
	
	        using(IConnection connection = factory.CreateConnection())
	        using(ISession session = connection.CreateSession())
	        {
	             // Examples for getting a destination:
	             //
	             // Hard coded destinations:
	             //    IDestination destination = session.GetQueue("FOO.BAR");
	             //    Debug.Assert(destination is IQueue);
	             //    IDestination destination = session.GetTopic("FOO.BAR");
	             //    Debug.Assert(destination is ITopic);
	             //
	             // Embedded destination type in the name:
	             //    IDestination destination = SessionUtil.GetDestination(session, "queue://FOO.BAR");
	             //    Debug.Assert(destination is IQueue);
	             //    IDestination destination = SessionUtil.GetDestination(session, "topic://FOO.BAR");
	             //    Debug.Assert(destination is ITopic);
	             //
	             // Defaults to queue if type is not specified:
	             //    IDestination destination = SessionUtil.GetDestination(session, "FOO.BAR");
	             //    Debug.Assert(destination is IQueue);
	             //
	             // .NET 3.5 Supports Extension methods for a simplified syntax:
	             //    IDestination destination = session.GetDestination("queue://FOO.BAR");
	             //    Debug.Assert(destination is IQueue);
	             //    IDestination destination = session.GetDestination("topic://FOO.BAR");
	             //    Debug.Assert(destination is ITopic);
	
	            IDestination destination = SessionUtil.GetDestination(session, "queue://FOO.BAR");
	            Console.WriteLine("Using destination: " + destination);
	
	            // Create a consumer and producer
	            using(IMessageConsumer consumer = session.CreateConsumer(destination))
	            using(IMessageProducer producer = session.CreateProducer(destination))
	            {
	                // Start the connection so that messages will be processed.
	                connection.Start();
	                //producer.Persistent = true;
	
	                // Send a message
	                ITextMessage request = session.CreateTextMessage("Hello World!");
	                request.NMSCorrelationID = "abc";
	                request.Properties["NMSXGroupID"] = "cheese";
	                request.Properties["myHeader"] = "Cheddar";
	
	                producer.Send(request);
	
	                // Consume a message
	                ITextMessage message = consumer.Receive() as ITextMessage;
	                if(message == null)
	                {
	                    Console.WriteLine("No message received!");
	                }
	                else
	                {
	                    Console.WriteLine("Received message with ID:   " + message.NMSMessageId);
	                    Console.WriteLine("Received message with text: " + message.Text);
	                }
	            }
	        }
	    }
	}
}

