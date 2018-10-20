using Chat.Model.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Model
{
    public class LocalClient : IClient
    {
        public LocalClient()
        {
            Connections = new List<ConnectionUtils>();
            _localConnection = new ConnectionUtils(new IPEndPoint(IPAddress.Loopback, 65000));
            _localConnection.MessageRecieved += OnMessageRecieved;
        }
        public string Name { get; set; }
        public bool CreateConnection(IPEndPoint endPoint)
        {
            if (Connections == null)
                Connections = new List<ConnectionUtils>();
            var connection = new ConnectionUtils(endPoint);
            if(connection.recieveSocketConnected && connection.sendSocketConnected)
                 connection.Disconnected += OnSocketDisconnected;
                 Connections.Add(connection);
            return connection.recieveSocketConnected && connection.sendSocketConnected;
        }
        public bool AddConenction(Socket socket)
        {
            var connection = new ConnectionUtils(socket);
            if (connection.recieveSocketConnected & connection.sendSocketConnected)
            {
                connection.Disconnected += OnSocketDisconnected;
                Connections.Add(connection);
            }
            return connection.recieveSocketConnected && connection.sendSocketConnected;
        }
        public void SendMessage(string message)
        {
            string Message = string.Format($"[{DateTime.Now.ToShortTimeString()}] {Name}: {message}");
            foreach(var conn in Connections)
                conn.SendMessage(Message, BasicTypes.MessageType.TextMessage);
        }
        public List<ConnectionUtils> Connections { get; private set; }
        public EventHandler ClientDisconnected;
        public EventHandler MessageRecieved;
        private void OnSocketDisconnected(object sender, EventArgs e)
        {
            var connection = sender as ConnectionUtils;
            Connections.Remove(connection);
            ClientDisconnected.Invoke(connection, new EventArgs());
        }
        private void OnMessageRecieved(object sender, EventArgs e)
        {
            MessageRecieved.Invoke(sender, e);
        }
        private ConnectionUtils _localConnection;
    }
}
