using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using RemoteClass;

namespace Client
{
	class Client
	{
		public static void Main(string [] args)
		{                 
			RemotingConfiguration.Configure("Client.exe.config", false);
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

