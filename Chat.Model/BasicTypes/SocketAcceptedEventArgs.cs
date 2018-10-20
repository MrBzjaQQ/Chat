using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.BasicTypes
{
    public class SocketAcceptedEventArgs: EventArgs
    {
        public Socket socket;
        public SocketAcceptedEventArgs(Socket _socket)
        {
            socket = _socket;
        }
    }
}
