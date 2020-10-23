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
                        if (Server.game != null)
                        {
                            DataPacket<JoinGamePacket> d = data.GetData<JoinGamePacket>();

                            Console.WriteLine("Player2 joined the game");
                            Server.game.AddPlayer(this, d.data.name, false);

                            SendData(new DataPacket<JoinGameResponse>()
                            {
                                type = "JOINGAMERESPONSE",
                                data = new JoinGameResponse()
                                {
                                    joinedGame = true
                                }
                            });

                            Console.WriteLine("Choose the cells");
                            Server.game.gameState = GameState.ChooseCells;

                            foreach (Player player in Server.game.players)
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
                        }
                        else
                        {
                            SendData(new DataPacket<JoinGameResponse>()
                            {
                                type = "JOINGAMERESPONSE",
                                data = new JoinGameResponse()
                                {
                                    joinedGame = false
                                }
                            });
                        }
                        break;
                    }
                case "HOSTGAME":
                    {
                        DataPacket<HostGamePacket> d = data.GetData<HostGamePacket>();

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
                        break;
                    }
                case "CELL":
                    {
                        DataPacket<CellPackage> d = data.GetData<CellPackage>();

                        if (Server.game.gameState == GameState.ChooseCells)
                        {
                            if (d.data.cell > 25)
                            {
                                if (d.data.isPlayer1)
                                {
                                    Console.WriteLine($"Player1 choose {d.data.cell}");
                                    if (Server.game.player1Grid.Count < 3 && Server.game.player1Grid.Count != 3 && !Server.game.player1Grid.ContainsKey(d.data.cell.ToString()))
                                    {
                                        Console.WriteLine("hij voegt hem bij p1 toe");
                                        Server.game.player1Grid.Add(d.data.cell.ToString(), false);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Player2 choose {d.data.cell}");
                                    if (Server.game.player2Grid.Count < 3 && Server.game.player2Grid.Count != 3 && !Server.game.player2Grid.ContainsKey(d.data.cell.ToString()))
                                    {
                                        Console.WriteLine("hij voegt hem bij p2 toe");
                                        Server.game.player2Grid.Add(d.data.cell.ToString(), false);
                                    }
                                }
                                
                                if (Server.game.player1Grid.Count == 3 && Server.game.player2Grid.Count == 3)
                                {
                                    Console.WriteLine("both players choose 3 cells, lets begin");
                                    Server.game.gameState = GameState.Player1Turn;

                                    foreach (Player player in Server.game.players)
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
                                }
                            }
                            break;
                        }
                        else if (Server.game.gameState == GameState.Player1Turn)
                        {
                            if (d.data.cell < 26)
                            {
                                bool hit = false;
                                int cell = d.data.cell + 25;
                                if (Server.game.player2Grid.ContainsKey(cell.ToString()))
                                {
                                    Console.WriteLine("Player 1 heeft geraakt");
                                    hit = true;
                                    Server.game.player2Grid[cell.ToString()] = true;
                                }
                                else
                                {
                                    Console.WriteLine("Player 1 heeft gemist");
                                }

                                Server.game.gameState = GameState.Player2Turn;
                                Server.game.CheckWinner(Server.game.player2Grid);

                                foreach (Player player in Server.game.players)
                                {
                                    SendData(player.client, new DataPacket<HitMissResponse>()
                                    {
                                        type = "HITMISSRESPONSE",
                                        data = new HitMissResponse()
                                        {
                                            hit = hit,
                                            cell = cell.ToString()
                                        }
                                    });
                                }

                                foreach (Player player in Server.game.players)
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
                            }
                            break;
                        } else if(Server.game.gameState == GameState.Player2Turn)
                        {
                            if (d.data.cell < 26)
                            {
                                bool hit = false;
                                int cell = d.data.cell + 25;
                                if (Server.game.player1Grid.ContainsKey(cell.ToString()))
                                {
                                    Console.WriteLine("Player 2 heeft geraakt");
                                    hit = true;
                                    Server.game.player1Grid[cell.ToString()] = true;
                                }
                                else
                                {
                                    Console.WriteLine("Player 2 heeft gemist");
                                }

                                Server.game.gameState = GameState.Player1Turn;
                                Server.game.CheckWinner(Server.game.player1Grid);

                                foreach (Player player in Server.game.players)
                                {
                                    SendData(player.client, new DataPacket<HitMissResponse>()
                                    {
                                        type = "HITMISSRESPONSE",
                                        data = new HitMissResponse()
                                        {
                                            hit = hit,
                                            cell = cell.ToString()
                                        }
                                    });
                                }

                                foreach (Player player in Server.game.players)
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
                            }
                            break;
                        }
                        break;
                    }
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

            private void SendData(ServerClient client, DataPacket<HitMissResponse> data)
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
    }
}
