using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GUI.Models
{
    public class Player : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }

        public bool isPlayer1 { get; set; }

        public bool Turn { get; set; }

        public string[] boatPositions { get; set; }

        public Player(string name, bool isPlayer1)
        {
            this.Name = name;
            this.isPlayer1 = isPlayer1;
            this.Turn = false;
            this.boatPositions = new string[3];
        }
    }
}
