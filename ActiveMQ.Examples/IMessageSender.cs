using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.NMS;

namespace ActiveMQ.Examples
{
    interface IMessageSender : IDisposable
    {
        string Destination { get; }
        IMessageProducer Producer { get; }

        void SendMessage(string message);
    }
}
