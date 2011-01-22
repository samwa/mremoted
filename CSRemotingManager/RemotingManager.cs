using System;
using System.Runtime.Remoting;

namespace Samwa.Masters
{	
	/// <summary>
	/// manger class that can create a remoting client or a remoting server
	/// </summary>
	public class RemotingManager
	{		
		private ServiceManager _remoteService = null;
		private CSClient _client = null;
		private CSServer _server = null;
		private string[] _args;
		
		public RemotingManager (string[] args)
		{			
			_args = args;
			//RemotingConfiguration.Configure("Client.exe.config", false);
		}
		
		public ServiceManager Setup(string clientConfig, string serverConfig)   
		{			
			_remoteService = new ServiceManager(); // the object to be shared
			
			if( _remoteService.Equals(null) )
			{
				// create a server
				_server = new CSServer();
				_server.Configure(serverConfig);
			}
			else
			{
				// create a client
				string strArgs;
				if (_args.Length == 0)
				{
					strArgs = "Client 1";
				}
				else
				{
					strArgs = _args[0];
				}
				
				_remoteService.WriteName(strArgs);
                //Console.WriteLine(remoteObj.ReadWelcome());
				//Console.ReadLine();
			}       
			
			return _remoteService;			
		}
		
		protected string WriteLine()
		{
			return String.Empty;
		}
	}
}

