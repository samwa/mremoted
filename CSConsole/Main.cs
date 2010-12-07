using System;
using System.Runtime.Remoting;

using ClientServer;
using CSRemotingManager;

namespace CSConsole
{
	class MainClass
	{
		private RemotingManager _csManager = null;
		private CSServer _csServer = null;
		private CSClient _csClient = null;
		
		public static void Main (string[] args)
		{     
			_csManager = new RemotingManager();
			
		}
	}
}

