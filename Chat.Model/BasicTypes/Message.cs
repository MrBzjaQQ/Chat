using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model.BasicTypes
{
    public class Message
    {
        //Поля в порядке следования
        public Message(int messageID, MessageType messageType, byte[] data)
        {
            this.messageID = messageID;
            this.messageType = messageType;
            this.data = data;
        }
        public byte[] GetBytes()
        {
            List<byte> mes = new List<byte>();
            mes.AddRange(BitConverter.GetBytes(messageID));
            mes.AddRange(BitConverter.GetBytes((int)messageType));
            mes.AddRange(data);
            return mes.ToArray();
        }
        public string GetDataString()
        {
            return Encoding.UTF8.GetString(data);
        }
        public int messageID; //4 Байта
        public MessageType messageType; //4 Байта
        public byte[] data;
    }
}
