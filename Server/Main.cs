using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;

namespace Server
{
	class Server
	{
		public static void Main (string[] args)
		{
			RemotingConfiguration.Configure("Server.exe.config", false);
			System.Console.WriteLine("The Server is ready .... Press the enter key to exit...");
			System.Console.ReadLine();
		}
	}
}

