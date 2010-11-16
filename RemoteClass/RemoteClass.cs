using System;
namespace RemoteClass
{
	public class RemoteClass : MarshalByRefObject
	{       
		private static string strWelcomeClient;   
		
		public RemoteClass()
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

