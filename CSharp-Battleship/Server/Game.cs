using System;
using System.Collections.Generic;
using System.Text;

namespace ServerApplication
{
    class Game
    {
        public List<Player> players;
        public Dictionary<string, bool> player1Grid { get; set; }
        public Dictionary<string, bool> player2Grid { get; set; }
        public GameState gameState { get; set; }

        public Game(ServerClient player, string name, bool isPlayer1)
        {
            player1Grid = new Dictionary<string, bool>();
            player2Grid = new Dictionary<string, bool>();
            this.players = new List<Player>();
            this.players.Add(new Player(player, name, isPlayer1));
        }

        public void AddPlayer(ServerClient client, string name, bool isPlayer1)
        {
            this.players.Add(new Player(client, name, isPlayer1));
        }

        public void CheckWinner(Dictionary<string, bool> grid)
        {
            int amount = 0;
            foreach(KeyValuePair<string, bool> cell in grid)
            {
                if (cell.Value)
                {
                    amount++;
                }
            }
            if(amount == 3)
            {
                this.gameState = GameState.Ended;
            }
        }

       
    }
}
