
namespace Samwa
{
    using System;
    using System.Runtime.Remoting.Lifetime;

	public class RemoteClass : MarshalByRefObject, IDisposable
	{       
		private static string strWelcomeClient;   
		private int[] _clients = null;
		private int? _currentClientIndex = null;
        private int? _totalClients = 0;
		
		private int? _iteration = null; // the current iteration in calculating pie
		private double _pi = 4; // pie = 4 as first approximation
		
		public RemoteClass()
		{
            _totalClients = 0;
			Console.WriteLine("Object created by server");
		}

        public RemoteClass(bool isClient)
        {
            if (isClient) _totalClients++;
            Console.WriteLine("Object created by client");
        }

        public void AddClient()
        {
            _totalClients++;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
		
		public RemoteClass (int currentIteration, double currentPi)
		{
			int k = currentIteration;
			double newPi = Math.Pow(-1, k) / (2 * k + 1);
			
			_pi = newPi;
			_iteration++;
		}

        public string Hello()
        {
            Console.WriteLine(String.Format("Hello from Remote Class, {0} Clients connected", _totalClients));
            return String.Format("Hello from Remote Class, {0} Clients connected", _totalClients);
        }   
		
		public string ReadWelcome()
		{                 
			return strWelcomeClient;
		}   
		
		public void WriteName(string strNameFromClient)
		{                 
			strWelcomeClient = "HI " + strNameFromClient + ". Welcome to Remoting World!!";                 
		}

        void IDisposable.Dispose()
        {
            // When a client correctly lets us go,
            // we can lower our count.
            _totalClients -= 1;
        }
    }
}

