
namespace Samwa.Masters
{
    using System;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;

	class RemotingServer
	{
        public static RemoteClass rmt;

		public static void Main (string[] args)
		{

			RemotingConfiguration.Configure("RemotingServer.exe.config", false);
            rmt = new RemoteClass();
            RemotingServices.Marshal((rmt), "RemoteClass");

			Console.WriteLine("The Server is ready .... Press the enter key to exit...");
			Console.ReadLine();
		}
	}
}

