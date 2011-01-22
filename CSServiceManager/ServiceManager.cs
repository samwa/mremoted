using System;

namespace Samwa.Masters
{
	// the remote class that will manage the app
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

