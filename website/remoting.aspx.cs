using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Runtime.Remoting;
using System.Net;
using System.Net.Sockets;

namespace Samwa
{
    public partial class Remoting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RemoteClass rc;

            IPAddress[] IPs = Dns.GetHostAddresses("127.0.0.1");

            Socket s = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            try
            {
                s.Connect(IPs[0], 8888);
                rc = (RemoteClass)Activator.GetObject(typeof(RemoteClass), "tcp://localhost:8888/RemoteClass");

                lblTotalClients.Text = rc.TotalClients.ToString();
                lblPi.Text = rc.Pi.ToString();
                lblIteration.Text = rc.Iteration.ToString();
                lblElapsedTime.Text = rc.ElapsedTime.ToString();

                pnlRemoteClass.Visible = true;
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                pnlError.Visible = true;
            }
        }
    }
}