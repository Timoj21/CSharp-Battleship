using FileIO;
using GalaSoft.MvvmLight.Command;
using GUI.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace GUI.ViewModels
{
    class Scoreboardviewmodel : ObserverableObject
    {
        private FileWriteRead filewriteread { get; set; }
        private MainViewModel MainViewModel { get; set; }

        public List<string> scoreboard { get; set; }



        public Scoreboardviewmodel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;

            this.filewriteread = new FileWriteRead();
            scoreboard = new List<string>();
            this.filewriteread.ReadFromFile();
            scoreboard = (this.filewriteread.outcomes);

            //string filepath = @"C:\Users\jornn\Desktop\csharp repo 2\CSharp-Battleship\CSharp-Battleship\ScoreBoard.txt";
            //List<string> outcomes = new List<string>();
            //outcomes = File.ReadAllLines(filepath).ToList();

            //scoreboard = outcomes;

        }

    }
}

