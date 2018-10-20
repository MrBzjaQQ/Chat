using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.BasicTypes
{
    public class MessageRecievedEventArgs : EventArgs
    {
        public MessageRecievedEventArgs(Message mes)
        {
            message = mes;
        }
        public Message message;
    }
}
