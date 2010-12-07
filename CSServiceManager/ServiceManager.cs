using System;

namespace CSServiceManager
{
	// the remote class that will manager the app
	public class ServiceManager : MarshalByRefObject	
	{
		private static string strWelcomeClient;   
		public ServiceManager ()
		{
			Console.WriteLine("Object created");
		}
		
		public string ReadWelcome()
		{                 
			return strWelcomeClient;
		}   
		
		public void WriteName(string strNameFromClient)
		{                 
			strWelcomeClient = "HI " + strNameFromClient + ". Welcome to Remoting World!!";                 
		}
	}
}

