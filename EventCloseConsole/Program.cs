

namespace EventCloseConsole
{
    using System.Runtime.InteropServices;
    using System;

    class Program
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        delegate void CleanUpMethod();

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

        private static bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                    Console.WriteLine("Closing");
                    System.Threading.Thread.Sleep(500);
                    return false;
                default:
                    return true;
            }
        }

        static void Main(string[] args)
        {
            //Console.TreatControlCAsInput = true;
            Console.CancelKeyPress += delegate
            {
                Console.WriteLine("Clean-up code invoked in CancelKeyPress handler.");
                // The application terminates directly after executing this delegate.
            };
            //new ConsoleCancelEventHandler(Console_CancelKeyPress);

            CleanUpMethod cleanUp =
               delegate
               {
                   Console.WriteLine("Clean-up code invoked");
               };

            //Console.CancelKeyPress +=
            //       delegate
            //       {
            //           cleanUp();
            //           // The application terminates directly after executing this delegate.
            //       };

            _handler += new EventHandler(Handler);

            //SetConsoleCtrlHandler(_handler, true);
            Console.ReadLine();

           
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Closing2");
            System.Threading.Thread.Sleep(500);
        }
    }
}
