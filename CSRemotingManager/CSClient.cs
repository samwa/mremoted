using System;

namespace Samwa.Masters
{
    public class CSClient : CSEntity
	{
		public CSClient (string[] args)
		{
			RemoteClass remoteObj = new RemoteClass();
			
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
				
				//remoteObj.WriteName(strArgs);                        
				//System.Console.WriteLine(remoteObj.ReadWelcome());
				System.Console.ReadLine();
			}                 
			
		}

        public override string EntityName()
        {
            return "CSClient";
        }
	}
}

