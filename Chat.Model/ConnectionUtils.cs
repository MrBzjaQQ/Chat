using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Chat.Model.BasicTypes;
using System.Threading;

namespace Chat.Model
{
    public class ConnectionUtils: IDisposable
    {
        public bool sendSocketConnected => _sender.Connected;
        public bool recieveSocketConnected => _listener.Connected;
        public ConnectionUtils(IPEndPoint endpoint)
        {
            _sender = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _recievedMessages = new List<Message>();
            _sender.Connect(endpoint);
            _listener.Connect(endpoint);
            _listenThread = new Thread(() =>
            {
                do
                {
                    try
                    {
                        byte[] data = new byte[512];
                        var readEvent = new AutoResetEvent(false);
                        SocketAsyncEventArgs e = new SocketAsyncEventArgs()
                        {
                            UserToken = readEvent
                        };
                        e.SetBuffer(data, 0, 512);
                        e.Completed += OnMessageRecieved;
                        _listener.ReceiveAsync(e);
                        readEvent.WaitOne();
                    }
                    catch (Exception)
                    {
                        Disconnected.Invoke(this, new EventArgs());
                    }
                }
                while (recieveSocketConnected);

            });
            _listenThread.Start();
            
        }

        public ConnectionUtils(Socket receive)
        {
            _listener = receive;
            _sender = new Socket(SocketType.Dgram, ProtocolType.Udp);
            _sender.Connect(receive.RemoteEndPoint);
            _listenThread = new Thread(() =>
            {
                do
                {
                    try
                    {
                        byte[] data = new byte[512];
                        var readEvent = new AutoResetEvent(false);
                        SocketAsyncEventArgs e = new SocketAsyncEventArgs()
                        {
                            UserToken = readEvent
                        };
                        e.SetBuffer(data, 0, 512);
                        e.Completed += OnMessageRecieved;
                        _listener.ReceiveAsync(e);
                        readEvent.WaitOne();
                    }
                    catch (Exception)
                    {
                        Disconnected.Invoke(this, new EventArgs());
                    }
                }
                while (recieveSocketConnected);

            });
            _listenThread.Start();
        }

        public Message GetRecievedMessage()
        {
            return _recievedMessages.FirstOrDefault();
        }
        public void SendMessage(string message, MessageType type)
        {
            Message mes = new Message(_sentMessageCounter, type, Encoding.UTF8.GetBytes(message));
            var data = mes.GetBytes();
            try
            {
                _sender.Send(data);
                _sentMessageCounter++;
            }
            catch(Exception)
            {
                Disconnected.Invoke(this, new EventArgs());
            }
        }
        private void OnMessageRecieved(object sender, EventArgs e)
        {
            SocketAsyncEventArgs MesResEventArgs;
            if (e is SocketAsyncEventArgs)
            {
                MesResEventArgs = e as SocketAsyncEventArgs;
                    Message mes = new Message(BitConverter.ToInt32(MesResEventArgs.Buffer.ToList().GetRange(0, 4).ToArray(), 0),
                        (MessageType)BitConverter.ToInt32(MesResEventArgs.Buffer.ToList().GetRange(4, 4).ToArray(), 0),
                        MesResEventArgs.Buffer.ToList().GetRange(8, MesResEventArgs.Count - 8).ToArray());
                    //_recievedMessages.Add(mes);
                    if(mes.messageType != MessageType.Unknown)
                        MessageRecieved.Invoke(this, new MessageRecievedEventArgs(mes));
                _recievedMessageCounter++;
            }
        }

        public void Dispose()
        {
            _listenThread.Interrupt();
            _sender.Disconnect(false);
            _listener.Disconnect(false);
        }

        public EventHandler MessageSent;
        public EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public EventHandler Disconnected;
        private int _sentMessageCounter = 0;
        private int _recievedMessageCounter = 0;
        private Socket _listener;
        private Socket _sender;
        private Thread _listenThread;
        private List<Message> _recievedMessages;
    }
}
