using BattleshipLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace GUI
{
    public delegate void DataCallback(string data);
    public delegate void InGameCallback(bool state);
    public delegate void ChooseGridCallback(bool state);
    public delegate void ReadyUpCallback(bool state);
    public class Client
    {
        private TcpClient client;
        private NetworkStream stream;
        private byte[] buffer = new byte[4];

        public event DataCallback OnDataReceived;
        public event InGameCallback OnInGameReceived;
        public event ReadyUpCallback OnReadyUpReceived;


        private bool inGame = false;

        public Client()
        {
            this.client = new TcpClient();
            this.client.BeginConnect("localhost", 5252, new AsyncCallback(Connect), null);
        }

        private void Connect(IAsyncResult ar)
        {
            this.client.EndConnect(ar);
            this.stream = client.GetStream();
            this.stream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(ReceiveLengthInt), null);
        }

        private void ReceiveLengthInt(IAsyncResult ar)
        {
            int dataLength = BitConverter.ToInt32(this.buffer);

            this.buffer = new byte[dataLength];

            this.stream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(ReceiveData), null);
        }

        private void ReceiveData(IAsyncResult ar)
        {
            string data = System.Text.Encoding.ASCII.GetString(this.buffer);

            DataPacket dataPacket = JsonConvert.DeserializeObject<DataPacket>(data);
            HandleData(dataPacket);

            this.buffer = new byte[4];
            this.stream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(ReceiveLengthInt), null);
        }

        private void HandleData(DataPacket data)
        {
            switch (data.type)
            {
                case "HOSTGAMERESPONSE":
                    {
                        DataPacket<HostGameResponse> d = data.GetData<HostGameResponse>();
                        this.inGame = d.data.inGame;
                        OnInGameReceived?.Invoke(this.inGame);
                        break;
                    }
                case "JOINGAMERESPONSE":
                    {
                        DataPacket<JoinGameResponse> d = data.GetData<JoinGameResponse>();
                        this.inGame = d.data.inGame;
                        OnInGameReceived?.Invoke(this.inGame);
                        break;
                    }
                case "CHOOSEGRIDRESPONSE":
                    {
                        //DataPacket<ChooseGridResponse> d = data.GetData<ChooseGridResponse>();
                        
                        break;
                    }
                case "READYUPRESPONSE":
                    {
                        DataPacket<ReadyUpResponse> d = data.GetData<ReadyUpResponse>();
                        OnReadyUpReceived?.Invoke(d.data.ready);
                        break;
                    }
            }
        }

        internal void SendAttack(bool isPlayer1, string v)
        {
            SendData(new DataPacket<AttackPacket>()
            {
                type = "ATTACK",
                data = new AttackPacket()
                {
                    isPlayer1 = isPlayer1,
                    cell = v
                }
            }) ;
        }

        internal void SendReadyUp(bool isPlayer1, string[] boatPositions)
        {
            SendData(new DataPacket<ReadyUpPacket>()
            {
                type = "READYUP",
                data = new ReadyUpPacket()
                {
                    isPlayer1 = isPlayer1,
                    boatPositions = boatPositions
                }
            });
        }

        public void SendHostGame(string name)
        {
            SendData(new DataPacket<HostGamePacket>()
            {
                type = "HOSTGAME",
                data = new HostGamePacket()
                {
                    name = name
                }
            });
        }

        public void SendJoinGame(string name)
        {
            SendData(new DataPacket<JoinGamePacket>()
            {
                type = "JOINGAME",
                data = new JoinGamePacket()
                {
                    name = name
                }
            });
        }

        public void SendChooseGrid(string name, int game, Dictionary<string, bool> grid)
        {
            SendData(new DataPacket<ChooseGridPackage>()
            {
                type = "CHOOSEGRID",
                data = new ChooseGridPackage()
                {
                    name = name,
                    game = game,
                    grid = grid
                }
            });
        }

        public void SendData(DataPacket<AttackPacket> data)
        {
            // create the sendBuffer based on the message
            List<byte> sendBuffer = new List<byte>(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

            // append the message length (in bytes)
            sendBuffer.InsertRange(0, BitConverter.GetBytes(sendBuffer.Count));

            // send the message
            this.stream.Write(sendBuffer.ToArray(), 0, sendBuffer.Count);
        }

        public void SendData(DataPacket<ReadyUpPacket> data)
        {
            // create the sendBuffer based on the message
            List<byte> sendBuffer = new List<byte>(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

            // append the message length (in bytes)
            sendBuffer.InsertRange(0, BitConverter.GetBytes(sendBuffer.Count));

            // send the message
            this.stream.Write(sendBuffer.ToArray(), 0, sendBuffer.Count);
        }

        public void SendData(DataPacket<ChooseGridPackage> data)
        {
            // create the sendBuffer based on the message
            List<byte> sendBuffer = new List<byte>(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

            // append the message length (in bytes)
            sendBuffer.InsertRange(0, BitConverter.GetBytes(sendBuffer.Count));

            // send the message
            this.stream.Write(sendBuffer.ToArray(), 0, sendBuffer.Count);
        }

        public void SendData(DataPacket<HostGamePacket> data)
        {
            // create the sendBuffer based on the message
            List<byte> sendBuffer = new List<byte>(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

            // append the message length (in bytes)
            sendBuffer.InsertRange(0, BitConverter.GetBytes(sendBuffer.Count));

            // send the message
            this.stream.Write(sendBuffer.ToArray(), 0, sendBuffer.Count);
        }

        public void SendData(DataPacket<JoinGamePacket> data)
        {
            // create the sendBuffer based on the message
            List<byte> sendBuffer = new List<byte>(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

            // append the message length (in bytes)
            sendBuffer.InsertRange(0, BitConverter.GetBytes(sendBuffer.Count));

            // send the message
            this.stream.Write(sendBuffer.ToArray(), 0, sendBuffer.Count);
        }
    }
}
