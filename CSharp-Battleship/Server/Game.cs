using System;
using System.Collections.Generic;
using System.Text;

namespace ServerApplication
{
    class Game
    {
        public List<ServerClient> players;
        public Dictionary<string, bool> player1Grid { get; set; }
        public Dictionary<string, bool> player2Grid { get; set; }
        private bool isPlayer1 { get; set; }



        public Game(ServerClient player1)
        {
            player1Grid = new Dictionary<string, bool>();
            player2Grid = new Dictionary<string, bool>();
            this.players = new List<ServerClient>();
            this.players.Add(player1);
        }

        public void playerJoin(ServerClient player2)
        {
            this.players.Add(player2);
        }

       
    }
}
