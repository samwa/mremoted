using System;

namespace Samwa.Masters
{
	class MainClass
	{
		private static RemotingManager _csManager = null;
		
		public static void Main (string[] args)
		{     
			_csManager = new RemotingManager(args);
            ServiceManager _sm = _csManager.Setup("Server.exe.config", "Client.exe.config");
		}
	}
}

