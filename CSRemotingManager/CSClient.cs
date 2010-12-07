using System;

namespace CSRemotingManager
{
	public class CSClient
	{
		public CSClient ()
		{
			RemoteClass.RemoteClass remoteObj = new RemoteClass.RemoteClass();
			
			if( remoteObj.Equals(null) )
			{
				System.Console.WriteLine("Error: unable to locate server");
			}
			else
			{
				string strArgs;
				if (args.Length == 0)
				{
					strArgs = "Client";
				}
				else
				{
					strArgs = args[0];
				}
				
				remoteObj.WriteName(strArgs);                        
				System.Console.WriteLine(remoteObj.ReadWelcome());
				System.Console.ReadLine();
			}                 
			
		}
	}
}

