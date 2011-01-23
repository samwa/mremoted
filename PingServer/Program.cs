using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace PingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress[] IPs = Dns.GetHostAddresses("127.0.0.1");

                Socket s = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);
                Console.WriteLine("Establishing Connection to \"127.0.0.1\"");

                try
                {
                    s.Connect(IPs[0], 8888);
                }
                catch (Exception ex)
                {
                    // something went wrong
                }
        }
    }
}
