using System;
using System.Runtime.Remoting;

namespace Samwa.Masters
{
	public class CSServer : CSEntity
	{
		public CSServer ()
		{
		
		}
		
		public void Configure(string path)
		{
			RemotingConfiguration.Configure(path, false);
			System.Console.WriteLine("The Server is ready .... Press the enter key to exit...");
			System.Console.ReadLine();			
		}

        public override string EntityName()
        {
            return "CSServer";
        }
	}
}

