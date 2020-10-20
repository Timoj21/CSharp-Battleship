using System;
using System.Collections.Generic;
using System.Text;

namespace ServerApplication
{
    class Game
    {
        public List<ServerClient> players;
        private Dictionary<string, bool> player1Grid { get; set; }
        private Dictionary<string, bool> player2Grid { get; set; }
        private bool isPlayer1 { get; set; }



        public Game(ServerClient player1)
        {
            this.players = new List<ServerClient>();
            this.players.Add(player1);
        }

        public void playerJoin(ServerClient player2)
        {
            this.players.Add(player2);
        }
    }
}
