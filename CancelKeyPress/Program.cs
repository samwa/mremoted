using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CancelKeyPress
{
    class Program
    {
        delegate void CleanUpMethod();

        static void Main(string[] args)
        {
            FileStream fs = null;
            StreamWriter logWriter = null;

            string cleanUpLocation = "handler.";
            CleanUpMethod cleanUp =
                delegate
                {
                    Console.WriteLine("Clean-up code invoked in " + cleanUpLocation);
                    if (logWriter != null)
                        logWriter.Flush();
                    if (fs != null)
                        fs.Close();
                };

            try
            {
                Console.WriteLine(
                    "This shows use of a single, no-param clean-up handler.");
                Console.CancelKeyPress +=
                    delegate
                    {
                        cleanUp();
                        // The application terminates directly after executing this delegate.
                    };

                fs = new FileStream("sample.log", FileMode.Append, FileAccess.Write, FileShare.None);
                logWriter = new StreamWriter(fs);
                //ProcessIncomingMessages(logWriter);
            }
            finally
            {
                cleanUpLocation = "finally.";
                cleanUp();
            }

            Console.ReadLine();
        }
    }
}
