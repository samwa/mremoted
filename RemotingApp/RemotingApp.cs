
using System;

using System.Runtime.Remoting;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Samwa
{
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
        private static bool _serverRestarted = false;
        

        static void Main(string[] args)
        {
            // event handler for closing app
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            Console.WriteLine("starting remoting app, press q to exit");

            // check to see if this is a forced server
            bool forceServer = (args.Length > 0 && args[0] == "forceserver");
            
            // check if the server exists
            if (ServerExists() && !forceServer)
            {
                // we need to create a client
                Console.WriteLine("starting remoting client");
                _appType = AppType.Client;

                DoLoopClient();
            }
            else
            {
                // no server so lets make one
                Console.WriteLine("starting remoting server{0}", (forceServer ? " forced" : String.Empty));
                _appType = AppType.Server;

                if (forceServer)
                {
                    while (ServerExists())
                    {
                        // loop until the server dies
                    }
                    Setup(_serverAppConfig);
                }
                else
                {
                    Setup(_serverAppConfig);
                }

                string[] activationData = new string[] { };
                _rmtClass = (RemoteClass)RemotingFactory.GetRemoteObject("localhost", 8888, "RemoteClass", typeof(RemoteClass));

                _startTime = DateTime.Now;
                DoLoopServer();
            }

            Console.WriteLine("end remoting app");
            System.Threading.Thread.Sleep(500);
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
                    // server is closing, we need to alert the object that a new server needs to be created
                    _rmtClass.CloseServer();
                    break;
                case AppType.Client:
                    // client is closing, call the client closing method of the object
                    while (!_done)
                    {
                        try
                        {
                            if (_rmtClass.RequestLock(_clientId, true))
                            {
                                // close client
                                _rmtClass.CloseClient(_clientId);

                                // refresh object
                                _rmtClass = (RemoteClass)RemotingFactory.GetRemoteObject("localhost", 8888, "RemoteClass", typeof(RemoteClass));
                                _lclClass = new RemoteClass(_rmtClass);

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

                    break;
                default:
                    break;
            }
        }

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
                _rmtClass.ElapsedTime = DateTime.Now.Subtract(_startTime).TotalSeconds;
            }
        }

        private static void DoLoopClient()
        {
            while (!_done)
            {
                if (Console.KeyAvailable)
                {
                    HandleKeyPress();
                }

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
                        // store remote object locally
                        _lclClass = new RemoteClass(_rmtClass);
                        // check for closing server
                        if (_rmtClass.ServerClosing)
                        {
                            if (!_serverRestarted)
                            {
                                // we need to start up a new server instance
                                RestartServer();
                                // and try and repopulate the remote object from our local one
                                _rmtClass = (RemoteClass)RemotingFactory.GetRemoteObject("localhost", 8888, "RemoteClass", typeof(RemoteClass));
                                _rmtClass = _lclClass;

                                // set this to true so we only restart the server once
                                _serverRestarted = true;
                            }
                        }
                        else
                        {
                            // do calc
                            _rmtClass.Iterate();

                            // refresh object
                            _rmtClass = (RemoteClass)RemotingFactory.GetRemoteObject("localhost", 8888, "RemoteClass", typeof(RemoteClass));
                            _lclClass = new RemoteClass(_rmtClass);

                            if (_serverRestarted)
                                _serverRestarted = false;

                        }
                        // release lock from server
                        _rmtClass.ReleaseLock(_clientId);
                    }

                }
                catch (SocketException soExp)
                {
                    if (!ServerExists())
                    {
                        // oops server has gone, we need to create another one
                        if (_lclClass.Clients.Length > 0 && _lclClass.Clients[0] == _clientId) // only restart the server if we are the first client in the list
                        {
                            RestartServer();
                            // and try and repopulate the remote object from our local one
                            _rmtClass = (RemoteClass)RemotingFactory.GetRemoteObject("localhost", 8888, "RemoteClass", typeof(RemoteClass));
                            _rmtClass = _lclClass;
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
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
