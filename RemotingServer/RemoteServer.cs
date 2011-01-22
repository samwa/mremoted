
namespace Samwa
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

            ListClientActivatedServiceTypes();
            ListWellKnownServiceTypes();
			Console.WriteLine("The Server is ready .... Press the enter key to exit...");
			Console.ReadLine();
		}
    
	    private static void ListClientActivatedServiceTypes()
	    {
		    foreach(ActivatedServiceTypeEntry entry in RemotingConfiguration.GetRegisteredActivatedServiceTypes())
		    {
                Console.WriteLine("Registered ActivatedServiceType: " + entry.TypeName);
		    }
	    }
        private static void ListWellKnownServiceTypes()
        {
            foreach (WellKnownServiceTypeEntry entry in RemotingConfiguration.GetRegisteredWellKnownServiceTypes())
            {
                Console.WriteLine(entry.TypeName + " is available at " + entry.ObjectUri);
            }
        }
	}
}

