using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;

namespace ActiveMQ.Examples
{
    class QueueReceiver : IMessageReceiver
    {
        public event MessageRecievedDelegate OnMessageRecieved;
        private readonly ISession session;
        private readonly string destination;
        private readonly IQueue queue;
        private bool disposed;

        public IMessageConsumer Consumer { get; private set; }
        public string ConsumerId { get; private set; }


        public QueueReceiver(ISession session, string destination)
        {
            this.session = session;
            this.destination = destination;
            queue = new ActiveMQQueue(destination);
        }

        public void Start(string consumerId)
        {
            Consumer = session.CreateConsumer(queue);
            Consumer.Listener += (message =>
                {
                    var textMessage = message as ITextMessage;
                    if (textMessage == null) throw new InvalidCastException();
                    if (OnMessageRecieved != null)
                    {
                        OnMessageRecieved(textMessage.Text);
                    }
                });
        }

        public void Dispose()
        {
            if (disposed) return;
            Consumer.Dispose();
            disposed = true;
        }
    }
}
