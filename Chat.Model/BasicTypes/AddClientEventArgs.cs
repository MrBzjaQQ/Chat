using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.BasicTypes
{
    public class AddClientEventArgs: EventArgs
    {
        public string IPAddress;
        public int Port;
        public AddClientEventArgs(string ipaddress, int port)
        {
            IPAddress = ipaddress;
            Port = port;
        }
    }
}
