using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.BasicTypes
{
    public class MessageSentEventArgs: EventArgs
    {
        public string Message;
        public MessageSentEventArgs(string message)
        {
            Message = message;
        }
    }
}
