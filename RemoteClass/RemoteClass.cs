
namespace Samwa
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Remoting.Lifetime;
    using System.Collections;
    using System.Linq;

    /// <summary>
    /// singleton class created by server
    /// used to calculate pi
    /// </summary>
    [Serializable]
	public class RemoteClass : MarshalByRefObject, IDisposable
	{   
        private int[] _clients = new int[] {};
        private int _totalClients = 0;
        private int _locked = 0; // anything > 0 means locked to that client, 0 means not locked, -1 means app is doing initialisation
        private int _nextClient = 1; // which client can use the system?
        private bool _serverClosing = false; // if true the the server is shutting down
        
        Queue _clientRequest; // for async requests, not implemented
		
		private int _iteration = 1; // the current iteration in calculating pi
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
            this.Locked = another.Locked;
            this.NextClient = another.NextClient;
            this.ServerClosing = another.ServerClosing;
            this.Pi = another.Pi;
            this.Iteration = another.Iteration;
            this.ElapsedTime = another.ElapsedTime;
        }

        #region public properties
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
            get  { return _totalClients; }
            private set { _totalClients = value; }
        }
        
        public int Locked {
            get { return _locked; }
            private set { _locked = value; }
        }

        public int NextClient
        {
            get { return _nextClient; }
            private set { _nextClient = value; }
        }

        public bool ServerClosing 
        {
            get { return _serverClosing; }
            private set { _serverClosing = value; } 
        }

        public double Pi
        {
            get { return _pi; }
            private set { _pi = value; }
        }

        public int Iteration
        {
            get { return _iteration; }
            private set { _iteration = value; }
        }

        public double ElapsedTime { get; set; }
        #endregion

        #region public methods
        public int AddClient()
        {
            List<int> list = new List<int>(_clients);

            int newId = (_clients.Length == 0 ? 1 : _clients.Max() + 1);

            list.Add(newId);

            _totalClients = list.Count;

            _clients = list.ToArray();

            Console.WriteLine("Adding client, total clients: {0}", _totalClients.ToString());
            return newId;
        }

        public void AddClient(int clientId)
        {
            List<int> list = new List<int>(_clients);
            list.Add(clientId);

            _totalClients = list.Count;

            _clients = list.ToArray();
        }

        public bool CloseClient(int clientId)
        {
            // remove this client id from clients array
            List<int> list = new List<int>(_clients);
            if (list.Remove(clientId))
            {
                _clients = list.ToArray();
                _totalClients = list.Count;
                return true;
            }

            return false;
        }

        // 
        public bool RequestLock(int clientId)
        {
            return RequestLock(clientId, false);
        }

        /// <summary>
        /// request a lock on the remoting object
        /// </summary>
        /// <param name="clientId">the client id doing the request</param>
        /// <param name="force">do we want to force the lock</param>
        /// <returns></returns>
        public bool RequestLock(int clientId, bool force)
        {
            if ((_locked == clientId || _locked == 0) // allowed to lock if already locked by client or not already locked
                && (_nextClient == clientId || force)) // allowed to lock if the client is the next or this is a forced request
            {
                _locked = clientId;
                return true;
            }

            return false;
        }
		
        public void Iterate()
        {
            int k = _iteration;
            double modifier = 4 * ( Math.Pow(-1, k) / ((2 * k) + 1) );
            double newPi = _pi + modifier;

            _pi = newPi;
            _iteration++;
        }

        public bool SetNextClient()
        {
            List<int> list = new List<int>(_clients);

            if (_locked == 0)
                return false;

            int currentClientIndex = list.IndexOf(_locked);

            if (list.Count <= currentClientIndex + 1)
                _nextClient = list[0];
            else
                _nextClient = list[currentClientIndex + 1];

            return true;
        }

        public void ReleaseLock(int clientId)
        {
            if (_locked == clientId)
                _locked = 0;
        }

        public void CloseServer()
        {
            _serverClosing = true;
        }

        public void DumpData()
        {
            string clientsStr = String.Join(",", Clients.Select(x => x.ToString()).ToArray());
            Console.WriteLine("RemoteClass data: Clients:[{0}], TotalClients:{1}, NextClient:{2}, Locked:{3}, ServerClosing:{4}, Pi:{5}, Iteration:{6}, ElapsedTime:{7}", 
                clientsStr, TotalClients.ToString(), NextClient.ToString(), Locked.ToString(), ServerClosing.ToString(), Pi.ToString(), Iteration.ToString(), ElapsedTime.ToString());
        }

        public void ResetData(RemoteClass original)
        {
            this.Clients = original.Clients;
            this.TotalClients = original.TotalClients;
            this.Locked = original.Locked;
            this.NextClient = original.NextClient;
            this.ServerClosing = original.ServerClosing;
            this.Pi = original.Pi;
            this.Iteration = original.Iteration;
            this.ElapsedTime = original.ElapsedTime;
        }
        #endregion

        #region private methods
        #endregion

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

