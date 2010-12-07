using System;
using System.Runtime.Remoting;

using CSServiceManager;

namespace CSRemotingManager
{
	public class RemotingManager
	{		
		private ServiceManager _remoteService = null;
		
		public RemotingManager ()
		{			
			RemotingConfiguration.Configure("Client.exe.config", false);
		}
		
		protected ServiceManager Configure(string clientConfig, string serverConfig)
		{
			
			_remoteService = new ServiceManager();
			
			if( _remoteService.Equals(null) )
			{
				// create a server
				
			}
			else
			{
				string strArgs;
				if (args.Length == 0)
				{
					strArgs = "Client 1";
				}
				else
				{
					strArgs = args[0];
				}
				
				_remoteService.WriteName(strArgs);                        
				//Console.WriteLine(remoteObj.ReadWelcome());
				//Console.ReadLine();
			}                 
			
		}
		
		protected string WriteLine()
		{
			
		}
	}
}

