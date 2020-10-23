using System;
using System.Collections.Generic;
using System.Text;

namespace ServerApplication
{
    public class Player
    {
        public ServerClient client { get; set; }
        public string name { get; set; }
        private bool isPlayer1 { get; set; }

        public Player(ServerClient client, string name, bool isPlayer1)
        {
            this.client = client;
            this.name = name;
            this.isPlayer1 = isPlayer1;
        }

    }

}
