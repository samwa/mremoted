using System;
using System.Runtime.Remoting;

namespace CSRemotingManager
{
	public class CSServer
	{
		public CSServer ()
		{
		
		}
		
		protected static string Configure(string path)
		{
			RemotingConfiguration.Configure(path, false);
			System.Console.WriteLine("The Server is ready .... Press the enter key to exit...");
			System.Console.ReadLine();			
		}
	}
}

