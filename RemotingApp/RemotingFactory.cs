using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Samwa
{
    public class RemotingFactory
    {
        public static object GetRemoteObject(string ipAddress, int port, string className, Type type)
        {
            return Activator.GetObject(type, "tcp://" + ipAddress + ":" + port.ToString() + "/" + className);
        }
    }
}
