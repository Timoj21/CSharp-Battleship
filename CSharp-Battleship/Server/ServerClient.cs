using BattleshipLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ServerApplication
{
    public class ServerClient
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        private byte[] buffer = new byte[4];
        private bool isConnected;


        public ServerClient(TcpClient tcpClient)
        {
            this.isConnected = true;
            this.tcpClient = tcpClient;
            this.stream = this.tcpClient.GetStream();
            stream.BeginRead(this.buffer, 0, this.buffer.Length, new AsyncCallback(ReceiveLengthInt), null);
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
                case "JOINGAME":
                    {
                        DataPacket<JoinGamePacket> d = data.GetData<JoinGamePacket>();

                        Console.WriteLine("Player2 joined the game");
                        Server.game.AddPlayer(this, d.data.name, false);

                        Console.WriteLine("Choose the cells");
                        Server.game.gameState = GameState.ChooseCells;

                        foreach(Player player in Server.game.players)
                        {
                            SendGameChange(player.client, new DataPacket<GameStateChangePacket>()
                            {
                                type = "GAMESTATECHANGE",
                                data = new GameStateChangePacket()
                                {
                                    state = Server.game.gameState.ToString()
                                }
                            });
                        }
                        break;
                        //foreach (var game in Server.games)
                        //{
                        //    if (game.players.Count == 1)
                        //    {
                        //        game.playerJoin(this);

                        //        SendData(new DataPacket<JoinGameResponse>()
                        //        {
                        //            type = "JOINGAMERESPONSE",
                        //            data = new JoinGameResponse()
                        //            {
                        //                inGame = true
                        //            }
                        //        });
                        //        break;
                        //    }
                        //}
                        ////Stuur terug dat er geen games beschikbaar zijn
                        //SendData(new DataPacket<JoinGameResponse>()
                        //{
                        //    type = "JOINGAMERESPONSE",
                        //    data = new JoinGameResponse()
                        //    {
                        //        inGame = false
                        //    }
                        //});
                        //break;
                    }
                case "HOSTGAME":
                    {
                        DataPacket<HostGamePacket> d = data.GetData<HostGamePacket>();
                        //Server.games.Add(new Game(this));

                        Console.WriteLine("new game started by host");
                        Server.game = new Game(this, d.data.name, d.data.isPlayer1);
                        Server.game.gameState = GameState.Waiting;
                        Console.WriteLine("Waiting for player2");

                        SendGameChange(this, new DataPacket<GameStateChangePacket>()
                        {
                            type = "GAMESTATECHANGE",
                            data = new GameStateChangePacket()
                            {
                                state = Server.game.gameState.ToString()
                            }
                        });
                        //SendData(new DataPacket<HostGameResponse>()
                        //{
                        //    type = "HOSTGAMERESPONSE",
                        //    data = new HostGameResponse()
                        //    {
                        //        inGame = true
                        //    }
                        //});
                        break;
                    }
                //case "READYUP":
                //    {
                //        DataPacket<ReadyUpPacket> d = data.GetData<ReadyUpPacket>();
                //        if (d.data.isPlayer1)
                //        {
                //            Server.games[0].player1Grid.Add(d.data.boatPositions[0], false);
                //            Server.games[0].player1Grid.Add(d.data.boatPositions[1], false);
                //            Server.games[0].player1Grid.Add(d.data.boatPositions[2], false);
                //        } else if (!d.data.isPlayer1)
                //        {
                //            Server.games[0].player2Grid.Add(d.data.boatPositions[0], false);
                //            Server.games[0].player2Grid.Add(d.data.boatPositions[1], false);
                //            Server.games[0].player2Grid.Add(d.data.boatPositions[2], false);
                //        }

                //        if(Server.games[0].player1Grid.Count == 3 && Server.games[0].player2Grid.Count == 3)
                //        {
                //            foreach (ServerClient client in Server.games[0].players)
                //            {
                //                SendData(client, new DataPacket<ReadyUpResponse>()
                //                {
                //                    type = "READYUPRESPONSE",
                //                    data = new ReadyUpResponse()
                //                    {
                //                        ready = true
                //                    }
                //                });
                //            }
                //        } else
                //        {
                //            foreach (ServerClient client in Server.games[0].players)
                //            {
                //                SendData(client, new DataPacket<ReadyUpResponse>()
                //                {
                //                    type = "READYUPRESPONSE",
                //                    data = new ReadyUpResponse()
                //                    {
                //                        ready = false
                //                    }
                //                });
                //            }
                //        }
                //        break;
                //    }
                //case "BATTLELOGMESSAGE":
                //    {
                //        DataPacket<BattlelogPacket> d = data.GetData<BattlelogPacket>();
                //        foreach (ServerClient client in Server.games[0].players)
                //        {
                //            SendData(client, new DataPacket<BattlelogResponse>()
                //            {
                //                type = "BATTLELOGRESPONSE",
                //                data = new BattlelogResponse()
                //                {
                //                    message = d.data.message
                //                }
                //            });
                //        }
                //        break;
                //    }
                //case "ATTACK":
                //    {
                //        DataPacket<AttackPacket> d = data.GetData<AttackPacket>();
                //        bool hit = false;
                //        if (d.data.isPlayer1)
                //        {
                //            if (Server.games[0].player2Grid.ContainsKey(d.data.cell))
                //            {
                //                hit = true;
                //                Server.games[0].player2Grid[d.data.cell] = true;
                //            }
                //        } else if (!d.data.isPlayer1)
                //        {
                //            if (Server.games[0].player1Grid.ContainsKey(d.data.cell))
                //            {
                //                hit = true;
                //                Server.games[0].player1Grid[d.data.cell] = true;
                //            }
                //        }
                //        foreach (ServerClient client in Server.games[0].players)
                //        {
                //            SendData(client, new DataPacket<AttackResponse>()
                //            {
                //                type = "ATTACKRESPONSE",
                //                data = new AttackResponse()
                //                {
                //                    hit = hit
                //                }
                //            });
                //        }
                //        break;
                //    }
            }
        }

        private void SendGameChange(ServerClient client, DataPacket<GameStateChangePacket> data)
        {
            if (this.isConnected)
            {
                // create the sendBuffer based on the message
                List<byte> sendBuffer = new List<byte>(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

                // append the message length (in bytes)
                sendBuffer.InsertRange(0, BitConverter.GetBytes(sendBuffer.Count));

                // send the message
                client.stream.Write(sendBuffer.ToArray(), 0, sendBuffer.Count);
            }
        }

        private void SendData(ServerClient client, DataPacket<AttackResponse> data)
        {
            if (this.isConnected)
            {
                // create the sendBuffer based on the message
                List<byte> sendBuffer = new List<byte>(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

                // append the message length (in bytes)
                sendBuffer.InsertRange(0, BitConverter.GetBytes(sendBuffer.Count));

                // send the message
                this.stream.Write(sendBuffer.ToArray(), 0, sendBuffer.Count);
            }
        }

        private void SendData(ServerClient client, DataPacket<BattlelogResponse> data)
        {
            if (this.isConnected)
            {
                // create the sendBuffer based on the message
                List<byte> sendBuffer = new List<byte>(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

                // append the message length (in bytes)
                sendBuffer.InsertRange(0, BitConverter.GetBytes(sendBuffer.Count));

                // send the message
                client.stream.Write(sendBuffer.ToArray(), 0, sendBuffer.Count);
            }
        }

        private void SendData(ServerClient client, DataPacket<ReadyUpResponse> data)
        {
            if (this.isConnected)
            {
                // create the sendBuffer based on the message
                List<byte> sendBuffer = new List<byte>(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

                // append the message length (in bytes)
                sendBuffer.InsertRange(0, BitConverter.GetBytes(sendBuffer.Count));

                // send the message
                this.stream.Write(sendBuffer.ToArray(), 0, sendBuffer.Count);
            }
        }

        private void SendData(DataPacket<JoinGameResponse> data)
        {
            if (this.isConnected)
            {
                // create the sendBuffer based on the message
                List<byte> sendBuffer = new List<byte>(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

                // append the message length (in bytes)
                sendBuffer.InsertRange(0, BitConverter.GetBytes(sendBuffer.Count));

                // send the message
                this.stream.Write(sendBuffer.ToArray(), 0, sendBuffer.Count);
            }
        }

        private void SendData(DataPacket<HostGameResponse> data)
        {
            if (this.isConnected)
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
}
