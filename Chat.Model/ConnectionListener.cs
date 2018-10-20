using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Chat.Model.BasicTypes;

namespace Chat.Model
{
    public class ConnectionListener
    {
        public ConnectionListener()
        {
            _listener = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listen = new Task(() =>
            {
                while(true)
                {
                _listener.Listen(1);
                NewConnection.Invoke(this, new SocketAcceptedEventArgs(_listener.Accept()));
                }
            });
            listen.Start();
        }
        private Socket _listener;
        public EventHandler<SocketAcceptedEventArgs> NewConnection;
        private Task listen;
    }
}
