using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Chat.Model.BasicTypes;
using System.Threading;

namespace Chat.Model
{
    public class ConnectionListener: IDisposable
    {
        public ConnectionListener()
        {
            _listener = new TcpListener(new IPEndPoint(IPAddress.Loopback, 32098)); 
            _listener.Start();
            listen = new Thread(() =>
            {
                while (true)
                {
                    NewConnection.Invoke(this, new SocketAcceptedEventArgs(_listener.AcceptSocket()));
                }
            });   
        }
        private TcpListener _listener;
        public EventHandler<SocketAcceptedEventArgs> NewConnection;
        private Thread listen;

        public void Dispose()
        {
            listen.Interrupt();
        }
    }
}
