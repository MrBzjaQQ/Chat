using Chat.Model;
using Chat.Model.BasicTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chat
{
    public class ChatViewModel
    {
        public ChatViewModel()
        {
            _localClient = new LocalClient();
            _localClient.MessageRecieved += onMessageRecieved;
            _localClient.ClientAccepted += OnNewClientConnected;
            connections = new ObservableCollection<string>();
        }
        public string ChatHistory { get; set; }
        public string Message { get; set; }
        public string SelectedItem { get; set; }
        public ICommand SendMessage
        {
            get {
                return _sendMessage ?? (_sendMessage = new RelayCommand((@object) =>
                {
                    var client = _localClient as LocalClient;
                    client.SendMessage(Message);
                    Message = "";
                }));
            }
        }

        public ICommand AddConnection
        {
            get
            {
                return _addConnection ?? (_addConnection = new RelayCommand( @object => {
                    var client = _localClient as LocalClient;
                    AddConnectionViewModel vm = new AddConnectionViewModel();
                    vm.DialogAccepted += OnClientAdded;
                    AddConnectionView view = new AddConnectionView();
                    view.DataContext = vm;
                    view.ShowDialog();
                }));
            }
        }
        public ICommand DeleteConnection
        {
            get
            {
                return _deleteConnection ?? (_deleteConnection = new RelayCommand(@object => {
                    connections.Remove(SelectedItem);
                }));
            }
        }
        private void onMessageRecieved(object sender, EventArgs e)
        {
            if (e is MessageRecievedEventArgs)
            ChatHistory += (e as MessageRecievedEventArgs).message + "\n";
        }
        private void OnClientAdded(object sender, AddClientEventArgs e)
        {
            _localClient.CreateConnection(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(e.IPAddress), e.Port));
        }
        private void OnNewClientConnected(object sender, ClientAcceptedEventArgs e)
        {
            connections.Add(e.Client.Nickname);
        }
        private ObservableCollection<string> connections;
        private LocalClient _localClient;
        private ICommand _addConnection;
        private ICommand _sendMessage;
        private ICommand _deleteConnection;
    }
}
