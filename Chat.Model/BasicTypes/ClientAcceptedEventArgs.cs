﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.BasicTypes
{
    public class ClientAcceptedEventArgs: EventArgs
    {
        public Client Client;
        public ClientAcceptedEventArgs(Client client)
        {
            Client = client;
        }
    }
}
