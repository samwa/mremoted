using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.NMS;

namespace ActiveMQ.Examples
{
    interface IMessageReceiver : IDisposable
    {
        IMessageConsumer Consumer { get; }
        string ConsumerId { get; }

        void Start(string consumerId);

        event MessageRecievedDelegate OnMessageRecieved;
    }
}
