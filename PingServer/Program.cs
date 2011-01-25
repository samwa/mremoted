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
        private static IPAddress[] IPs;
        private static bool done;

        static void Main(string[] args)
        {
            IPs = Dns.GetHostAddresses("127.0.0.1");

            Socket s = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            Console.WriteLine("Establishing Connection to \"127.0.0.1\"");

            while (!done)
            {
                if (Console.KeyAvailable)
                {
                    HandleKeyPress();
                }
                Console.WriteLine("Server is {0}", (ServerExists() ? "up" : "down"));
                System.Threading.Thread.Sleep(1000);
            } 
        }

        private static void HandleKeyPress()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.Q:
                    done = true;
                    break;
                default:
                    break;
            }
        }

        private static bool ServerExists()
        {
            Socket s = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            try
            {
                s.Connect(IPs[0], 8888);
                return true;
            }
            catch (Exception ex)
            {
                // something went wrong
                return false;
            }

        }
    }
}
