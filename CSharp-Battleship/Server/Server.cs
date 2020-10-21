using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApplication
{
    class Server
    {
        private TcpListener listener;
        public static List<Game> games;
        public Server()
        {
            Console.WriteLine("Server starting...");

            games = new List<Game>();

            this.listener = new TcpListener(IPAddress.Any, 5252);
            this.listener.Start();
            Console.WriteLine("Server started");
            this.listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);

        }

        private void OnConnect(IAsyncResult ar)
        {
            var tcpClient = this.listener.EndAcceptTcpClient(ar);
            Console.WriteLine($"Client connected from {tcpClient.Client.RemoteEndPoint}");
            ServerClient serverclient = new ServerClient(tcpClient);
            this.listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }
    }
}
