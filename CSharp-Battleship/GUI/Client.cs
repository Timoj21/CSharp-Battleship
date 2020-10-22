using BattleshipLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;

namespace GUI
{
    public delegate void DataCallback(string data);
    public delegate void JoinedGameCallback(bool state);
    public delegate void ChooseGridCallback(bool state);
    public delegate void ReadyUpCallback(bool state);
    public delegate void BattlelogCallback(string message);
    public delegate void AttackCallback(bool hit);

    public delegate void GameStateChangeCallback(string gameState);

    public delegate void HitMissCallback(string cell, bool hit);
    public class Client
    {
        private TcpClient client;
        private NetworkStream stream;
        private byte[] buffer = new byte[4];

        public event GameStateChangeCallback OnGameStateChangeReceived;
        public event JoinedGameCallback OnJoinedGameReceived;
        public event HitMissCallback OnHitMissReceived;

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
                        OnJoinedGameReceived?.Invoke(d.data.inGame);
                        break;
                    }
                case "GAMESTATECHANGE":
                    {
                        DataPacket<GameStateChangePacket> d = data.GetData<GameStateChangePacket>();
                        OnGameStateChangeReceived?.Invoke(d.data.state);
                        break;
                    }
                case "JOINGAMERESPONSE":
                    {
                        DataPacket<JoinGameResponse> d = data.GetData<JoinGameResponse>();
                        OnJoinedGameReceived?.Invoke(d.data.joinedGame);
                        break;
                    }
                case "HITMISSRESPONSE":
                    {
                        DataPacket<HitMissResponse> d = data.GetData<HitMissResponse>();
                        OnHitMissReceived?.Invoke(d.data.cell, d.data.hit);
                        break;
                    }
            }
        }

        internal void SendCell(bool isPlayer1, string cell)
        {
            SendData(new DataPacket<CellPackage>()
            {
                type = "CELL",
                data = new CellPackage()
                {
                    isPlayer1 = isPlayer1,
                    cell = cell
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
                    name = name,
                    isPlayer1 = true
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

        public void SendData(DataPacket<CellPackage> data)
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
