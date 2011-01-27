using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apache.NMS;
using Apache.NMS.Util;
using System.Threading;

namespace website
{
    public partial class queue : System.Web.UI.Page
    {
        protected static AutoResetEvent semaphore = new AutoResetEvent(false);
        protected static ITextMessage message = null;
        protected static TimeSpan receiveTimeout = TimeSpan.FromSeconds(10);

        protected void Page_Load(object sender, EventArgs e)
        {
            Uri connectUri = new Uri("activemq:tcp://localhost:61616");

            IConnectionFactory factory = new NMSConnectionFactory(connectUri);

            IConnection connection = factory.CreateConnection();
            using (ISession session = connection.CreateSession())
            {
                IDestination destination = SessionUtil.GetDestination(session, "queue://FOO.BAR");

                IMessageConsumer consumer = session.CreateConsumer(destination);
                using (IMessageProducer producer = session.CreateProducer(destination))
                {
                    connection.Start();
                    //producer.per
                    producer.RequestTimeout = receiveTimeout;
                    consumer.Listener += new MessageListener(OnMessage);

                    ITextMessage request = session.CreateTextMessage("Hello World");
                    request.NMSCorrelationID = "abc";
                    request.Properties["NMSXGroupID"] = "cheese";
                    request.Properties["myHeader"] = "Cheddar";

                    producer.Send(request);

                    semaphore.WaitOne((int)receiveTimeout.TotalMilliseconds, true);
                    if (message == null)
                    {
                    }
                    else
                    {
                        string messageId = message.NMSMessageId;
                        string messageText = message.Text;
                    }

                }

            }
        }

        void OnMessage(IMessage receivedMsg)
        {
            message = receivedMsg as ITextMessage;
            semaphore.Set();
        }
    }
}