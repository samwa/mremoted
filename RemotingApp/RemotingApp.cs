
using System;

using System.Runtime.Remoting;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;

namespace Samwa
{
    class RemotingApp
    {
        private static bool _done = false;
        private static int _clientId;
        private static string _serverAppConfig = "ServerApp.config";
        private static string _clientAppConfig = "ClientApp.config";
        private static RemoteClass _rmtClass;
        private static RemoteClass _lclClass;

        static void Main(string[] args)
        {
            Console.WriteLine("starting remoting app, press q to exit");
            
            // check if the server exists
            if (ServerExists())
            {
                // we have a server, so lets create a client
                Console.WriteLine("starting remoting client");
                //Setup(_clientAppConfig); // no need to run setup, do it manually below

                string[] activationData = new string[]{};

                _rmtClass = (RemoteClass)RemotingFactory.GetRemoteObject("localhost", 8888, "RemoteClass", typeof(RemoteClass));
                _lclClass = new RemoteClass(_rmtClass);
                //_rmtClass = (RemoteClass)(Activator.CreateInstance(typeof(RemoteClass), activationData));

                _rmtClass.AddClient();

                //Console.WriteLine(_rmtClass.Hello());

                DoLoopClient();
            }
            else
            {
                // no serer so lets make one
                Console.WriteLine("starting remoting server");
                Setup(_serverAppConfig);

                string[] activationData = new string[] { };
                _rmtClass = (RemoteClass)(Activator.CreateInstance(typeof(RemoteClass), activationData));

                DoLoopServer();
            }

            Console.WriteLine("end remoting app");
            Console.ReadLine();
        }

        private static bool RestartServer()
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = "RemotingApp.exe";
            processInfo.RedirectStandardOutput = true;

            using (Process process = Process.Start(processInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result.Contains("starting remoting server");
                }
            }
        }

        private static bool ServerExists()
        {
            Console.WriteLine("Establishing Connection to \"127.0.0.1\"");

            IPAddress[] IPs = Dns.GetHostAddresses("127.0.0.1");

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
                return false;
            }
        }

        private static void Setup(string appConfig)
        {
            RemotingConfiguration.Configure(appConfig, false);

        }

        private static void DoLoopServer()
        {
            while (!_done)
            {
                if (Console.KeyAvailable)
                {
                    HandleKeyPress();
                }
            }
        }

        private static void DoLoopClient()
        {
            while (!_done)
            {
                try
                {
                    int totalClients = _rmtClass.TotalClients;
                    // request lock from server
                    _rmtClass.RequestLock(_clientId);

                    // do calc
                    _rmtClass.Iterate();

                    // release lock from server
                    _rmtClass.ReleaseLock(_clientId);

                }
                catch (Exception ex)
                {                    
                    if (!ServerExists())
                    {
                            // oops server has gone, we need to create another one
                        if (_lclClass.Clients.Length > 0 && _lclClass.Clients[0] == _clientId) // only restart the server if we are the first client in the list
                            RestartServer();
                    }
                    throw;
                }

                if (Console.KeyAvailable)
                {
                    HandleKeyPress();
                }
            }
        }

        private static void HandleKeyPress()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.Q:
                    _done = true;
                    break;
                case ConsoleKey.E:
                    throw new Exception("Forced Exception");
                default:
                    break;
            }
        }

    }
}
