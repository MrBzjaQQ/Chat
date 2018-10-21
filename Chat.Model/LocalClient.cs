using Chat.Model.BasicTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Model
{
    public class LocalClient
    {
        public LocalClient()
        {
            Clients = new List<Client>();
            _listener = new Listener();
            _listening = new Thread(() => _listener.StartListening());
            _listener.MessageRecieved += OnMessageRecieved;
            _listener.ClientAccepted += OnClientAccepted;
            Name = "Ilya";
        }
        public string Name { get; set; }
        public void SendMessage(string message)
        {
            string Message = string.Format($"[{DateTime.Now.ToShortTimeString()}] {Name}: {message}");
            foreach(var cl in Clients)
                cl.SendMessage(Message);
        }
        public List<Client> Clients { get; private set; }
        public EventHandler ClientDisconnected;
        public EventHandler MessageRecieved;
        public EventHandler<ClientAcceptedEventArgs> ClientAccepted;
        private void OnSocketDisconnected(object sender, EventArgs e)
        {
            var client = sender as Client;
            Clients.Remove(client);
            ClientDisconnected.Invoke(client, new EventArgs());
        }
        private void OnMessageRecieved(object sender, MessageRecievedEventArgs e)
        {
            MessageRecieved.Invoke(this, e);
        }
        private void OnClientAccepted(object sender, ClientAcceptedEventArgs e)
        {
            var client = e.Client;
            Clients.Add(client);
            ClientAccepted.Invoke(this, e);
            client.SendMessage($"{Name}<!Nickname>");
        }
        private Listener _listener;
        private Thread _listening;

        public void CreateConnection(IPEndPoint iPEndPoint)
        {
            Client.CreateConnection(Name,iPEndPoint);
        }
    }
}
