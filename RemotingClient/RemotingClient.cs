

namespace Samwa
{
    using System;
    using System.Runtime.Remoting;

    class RemotingClient
    {
        static void Main(string[] args)
        {
            RemotingConfiguration.Configure("RemotingClient.exe.config", false);

            RemoteClass rmt = (RemoteClass)(Activator.CreateInstance(typeof(RemoteClass), args));

            //RemoteClass rmt = new RemoteClass(true);

            bool objectCreated = (rmt != null);

            if (objectCreated)
            {
                rmt.AddClient();

                Console.WriteLine(rmt.Hello());

                Console.ReadLine();

            }
        }

    }
}
