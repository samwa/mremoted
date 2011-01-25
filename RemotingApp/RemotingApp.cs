

namespace Samwa
{
    using System;
    using System.Linq;
    using System.Runtime.Remoting;
    using System.Net;
    using System.Net.Sockets;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Tcp;

    class RemotingApp
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        enum AppType
        {
            Unknown = 0,
            Server = 1,
            Client = 2
        }

        private static bool _done = false;

        private static int _clientId = 0;
        private static string _serverAppConfig = "RemotingApp.exe.config";
        private static RemoteClass _rmtClass; // an instance of the remote class
        private static RemoteClass _lclClass; // a local instance of the remote class
        private static AppType _appType = AppType.Unknown;
        private static DateTime _startTime;
        private static bool _startingServer = false;

        static void Main(string[] args)
        {

            IChannel[] regChannels = ChannelServices.RegisteredChannels;
            if (regChannels.Length > 0)
            {
                IChannel channel = (IChannel)ChannelServices.GetChannel(regChannels[0].ChannelName);
            }

            // event handler for closing app
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            Console.WriteLine("starting remoting app, press q to exit");

            // check to see if this is a forced server
            bool forceServer = (args.Length > 0 && args[0] == "forceserver");
            
            // check if the server exists
            if (ServerExists())
            {
                // we need to create a client
                StartClient();
            }
            else
            {
                // no server so lets make one
                StartServer();
                //_startingServer = true;
            }

            if (_startingServer)
            {
            }

            Console.WriteLine("end remoting app");
            System.Threading.Thread.Sleep(500);
        }

        private static void StartClient()
        {
            Console.WriteLine("starting remoting client");
            _appType = AppType.Client;

            DoLoopClient();
        }

        private static void StartServer()
        {
            // make sure there isn't a server already
            int i = 0; // make sure we don't loop for ever
            while (ServerExists() && i < 100)
            {
                // just loop until the server goes down
                i++;
            }

            if (i > 99)
                throw new Exception("Cannot create server, existing server won't stop");

            Console.WriteLine("starting remoting server{0}");
            _appType = AppType.Server;

            Setup(_serverAppConfig);

            string[] activationData = new string[] { };
            _rmtClass = (RemoteClass)RemotingFactory.GetRemoteObject("localhost", 8888, "RemoteClass", typeof(RemoteClass));

            _startTime = DateTime.Now;
            DoLoopServer();
        }

        /// <summary>
        /// handler to handle closing the app
        /// </summary>
        /// <param name="sig"></param>
        /// <returns></returns>
        private static bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                    Cleanup();
                    Console.WriteLine("Closing");
                    System.Threading.Thread.Sleep(500);
                    return false;
                default:
                    return true;
            }
        }

        private static void Cleanup()
        {
            switch (_appType)
            {
                case AppType.Unknown:
                    // this should never get hit
                    Console.WriteLine("Closing - Unknown app type");
                    System.Threading.Thread.Sleep(750);

                    break;
                case AppType.Server:
                    CloseServer();

                    break;
                case AppType.Client:
                    CloseClient();                    

                    break;
                default:
                    break;
            }
        }

        private static void CloseServer()
        {
            // server is closing, we need to alert the object that a new server needs to be created
            _rmtClass.CloseServer();

            IChannel[] regChannels = ChannelServices.RegisteredChannels;
            if (regChannels.Length > 0)
            {
                Console.WriteLine("Unregistering tcp channel: {0}", regChannels[0].ChannelName);
                IChannel channel = (IChannel)ChannelServices.GetChannel(regChannels[0].ChannelName);
                ChannelServices.UnregisterChannel(channel);
                System.Threading.Thread.Sleep(100);
            }
        }

        private static void CloseClient()
        {
            // client is closing, call the client closing method of the object
            _done = false;
            while (!_done)
            {
                try
                {
                    if (_rmtClass.RequestLock(_clientId, true))
                    {
                        // close client
                        _rmtClass.CloseClient(_clientId);
                        
                        // release lock from server
                        _rmtClass.SetNextClient();
                        _rmtClass.ReleaseLock(_clientId);

                        _done = true;
                    }
                }
                catch (Exception ex)
                {
                    // exit anyway
                    _done = true;
                }
            }
        }

        //[Obsolete("app switches to be a server, rather then starting one")]
        private static bool RestartServer()
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = "RemotingApp.exe";
            processInfo.UseShellExecute = true;
            processInfo.RedirectStandardOutput = false;
            processInfo.Arguments = "forceserver";

            Process.Start(processInfo);
            return true;
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
            //TcpChannel channel = new TcpChannel(8888);
            //ChannelServices.RegisterChannel(channel, false);
            //RemotingConfiguration.RegisterWellKnownServiceType(
            //    typeof(RemoteClass), "RemoteClass",
            //    WellKnownObjectMode.Singleton);
            RemotingConfiguration.Configure(appConfig, false);
        }

        private static void DoLoopServer()
        {
            while (!_done)
            {
                _rmtClass.ElapsedTime = DateTime.Now.Subtract(_startTime).TotalSeconds;
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
                    if (_clientId == 0)
                    {
                        // we have a server, so lets create a client
                        _rmtClass = (RemoteClass)RemotingFactory.GetRemoteObject("localhost", 8888, "RemoteClass", typeof(RemoteClass));

                        _clientId = _rmtClass.AddClient();
                        _lclClass = new RemoteClass(_rmtClass);
                    }

                    // request lock from server
                    if (_rmtClass.RequestLock(_clientId))
                    {
                        if (_startingServer)
                        {
                            // we have just restarted the server, now we need to set the remote object back into it 

                            _startingServer = false;
                        }

                        // are we listed in the object?
                        if (!_rmtClass.Clients.Contains(_clientId))
                            _rmtClass.AddClient(_clientId);

                        // do calc
                        _rmtClass.Iterate();

                        _lclClass = new RemoteClass(_rmtClass);

                        // release lock from server
                        _rmtClass.ReleaseLock(_clientId);
                    }

                    if (Console.KeyAvailable)
                    {
                        HandleKeyPress();
                    }
                }
                catch (SocketException soExp)
                {
                    if (!ServerExists())
                    {
                        // oops server has gone, we need to create another one
                        if (IsFirstClient() && !_startingServer) // only switch on server if we are the first client in the list and not already starting the server
                        {
                            _startingServer = true;
                            RestartServer();
                        }
                    }
                    else
                    {
                        Cleanup();
                        throw;
                    }
                }
                catch
                {
                    Cleanup();
                    throw;
                }
            }
        }

        private static bool IsFirstClient()
        {
            if (_lclClass == null || _clientId == 0)
                return false;

            return (_lclClass.Clients.Length > 0 && _lclClass.Clients[0] == _clientId);
        }

        private static void HandleKeyPress()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.D:                    
                    _rmtClass.DumpData();
                    break;
                case ConsoleKey.Q:
                    Cleanup();
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
