using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chat.Model
{
    public class Client
    {
        public int ID;
        public string Nickname;
        public Socket Socket;
        public Client(int id, Socket socket, string nickname)
        {
            ID = id;
            Socket = socket;
            Nickname = nickname;
        }
        public static void CreateConnection(string nickname, IPEndPoint iPEndPoint)
        {
            try
            {
                Socket connect = new Socket(SocketType.Stream, ProtocolType.Tcp);
                connect.Connect(iPEndPoint);
                string Message = nickname + "<!Nickname>";
                byte[] data = Encoding.UTF8.GetBytes(Message);
                connect.BeginSend(data, 0, data.Length, 0, new AsyncCallback(AsyncSendCallback), connect);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void SendMessage(string message)
        {
            string Message = message + "<!Message>";
            byte[] data = Encoding.UTF8.GetBytes(Message);
            Socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), Socket);
        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSend = handler.EndSend(ar);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        private static void AsyncSendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSend = handler.EndSend(ar);
                handler.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        
    }
}
