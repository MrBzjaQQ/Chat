using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.BasicTypes
{
    public class ClientAcceptedEventArgs: EventArgs
    {
        public string IPAddress;
        public int Port;
        public ClientAcceptedEventArgs(string ipaddress, int port)
        {
            IPAddress = ipaddress;
            Port = port;
        }
    }
}
