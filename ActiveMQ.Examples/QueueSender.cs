using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;

namespace ActiveMQ.Examples
{
    public class QueueSender : IMessageSender
    {
        private bool disposed;
        private readonly IQueue queue;
        private readonly ISession session;

        public IMessageProducer Producer { get; private set; }

        public string Destination { get; private set; }

        public QueueSender(ISession session, string destination)
        {
            Destination = destination;
            this.session = session;

            queue = new ActiveMQQueue(Destination);
            Producer = this.session.CreateProducer(queue);
        }

        public void SendMessage(string message)
        {
            Producer.Send(session.CreateTextMessage(message));
        }

        public void Dispose()
        {
            if (disposed) return;
            Producer.Dispose();
            disposed = true;
        }
    }
}
