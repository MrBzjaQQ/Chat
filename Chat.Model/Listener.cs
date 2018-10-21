using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Chat.Model.BasicTypes;
using System.Threading;
using System.Windows;

namespace Chat.Model
{
    public class Listener
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public Listener()
        {
            connectionsCounter = 0;
            messagesCounter = 0;
        }
        public void StartListening()
        {
            IPHostEntry iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress = iPHostEntry.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(iPAddress, 11000);
            Socket listener = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);
                while (true)
                {
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }
        public void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = handler.EndReceive(ar);
            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
                content = state.sb.ToString();
                if(content.IndexOf("<!Nickname>") > -1)
                {
                    Client client = new Client(connectionsCounter, handler, content.Replace("<!Nickname>", ""));
                    ClientAccepted.Invoke(this, new ClientAcceptedEventArgs(client));
                    connectionsCounter++;
                }
                else if(content.IndexOf("<!Message>") > -1)
                {
                    MessageRecieved.Invoke(this, new MessageRecievedEventArgs(new Message(messagesCounter, MessageType.TextMessage, Encoding.UTF8.GetBytes(content))));
                    messagesCounter++;
                }
                else
                {
                    return;
                }
            }

        }

        public EventHandler<ClientAcceptedEventArgs> ClientAccepted;
        public EventHandler<MessageRecievedEventArgs> MessageRecieved;
        private int connectionsCounter;
        private int messagesCounter;

    }
}
