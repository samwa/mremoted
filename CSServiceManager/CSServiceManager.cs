using System;

namespace CSServiceManager
{
	public class CSServiceManager : MarshalByRefObject
	{
		private static string strWelcomeClient;   
		
		public CSServiceManager ()
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

