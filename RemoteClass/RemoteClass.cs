
namespace Samwa
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Remoting.Lifetime;
using System.Collections;

    /// <summary>
    /// singleton class created by server
    /// used to calculate pi
    /// </summary>
	public class RemoteClass : MarshalByRefObject, IDisposable
	{       
		private static string strWelcomeClient;   
		private int[] _clients = new int[] {};
        private int _totalClients = 0;
        private int _locked = 0; // anything > 0 means locked to that client, 0 means not locked
        Queue _clientRequest;
		
		private int _iteration = 0; // the current iteration in calculating pie
		private double _pi = 4; // pi = 4 as first approximation
		
		public RemoteClass()
		{
			Console.WriteLine("Object created");
		}

        /// <summary>
        /// for cloning
        /// </summary>
        /// <param name="another"></param>
        public RemoteClass(RemoteClass another)
        {
            this.Clients = another.Clients;
            this.TotalClients = another.TotalClients;
        }

        public int[] Clients
        {
            get
            {
                return _clients;
            }
            private set
            {
                _clients = value;
            }
        }

        public int TotalClients 
        { 
            get 
            {
                return _totalClients;
            }
            private set
            {
                _totalClients = value;
            }
        }

        public int AddClient()
        {
            List<int> list = new List<int>(_clients);
            list.Add(++_totalClients);

            _totalClients = list.Count;

            _clients = list.ToArray();

            Console.WriteLine("Adding client, total clients: {0}", _totalClients.ToString());
            return _totalClients;
        }

        // 
        public bool RequestLock(int clientId)
        {
            if (_locked == clientId || _locked == 0)
            {
                _locked = clientId;
                return true;
            }

            return false;
        }

        public void ReleaseLock(int clientId)
        {
            if (_locked == clientId)
                _locked = 0;
        }
		
        public void Iterate()
        {
            int k = _iteration;
            double newPi = _pi + ( Math.Pow(-1, k) / (2 * k + 1) );

            _pi = newPi;
            _iteration++;
        }

        void IDisposable.Dispose()
        {
            // When a client correctly lets us go,
            // we can lower our count.
            _totalClients -= 1;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}

