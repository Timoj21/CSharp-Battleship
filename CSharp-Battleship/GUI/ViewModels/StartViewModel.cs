using FileIO;
using GalaSoft.MvvmLight.Command;
using GUI.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class StartViewModel : ObserverableObject
    {
        private Client client;

        private MainViewModel MainViewModel { get; set; }
        public ICommand joinGameCommand { get; set; }

        public ICommand hostGameCommand { get; set; }

        public ICommand scoresGameCommand { get; set; }

        public string Name { get; set; }



        public StartViewModel(MainViewModel mainViewModel)
        {
            this.client = new Client();
            this.client.OnDataReceived += Client_OnDataReceived;
            this.client.OnInGameReceived += Client_OnInGameReceived;
            this.MainViewModel = mainViewModel;
            joinGameCommand = new RelayCommand(() =>
            {
                if (Name != null && Name.Length > 0)
                {
                    this.client.SendJoinGame(Name);
                }
            });

            hostGameCommand = new RelayCommand(() =>
            {
                if (Name != null && Name.Length > 0)
                {
                    this.client.SendHostGame(Name);
                }
            });

            scoresGameCommand = new RelayCommand(() =>
            {
                MainViewModel.SelectedViewModel = new Scoreboardviewmodel(this.MainViewModel);
            });
        }

        private void Client_OnInGameReceived(bool state)
        {
            if (state)
            {
                MainViewModel.SelectedViewModel = new GameViewModel(this.MainViewModel);
                MainViewModel.players.Add(new Models.Player(Name));
            }
        }

        private void Client_OnDataReceived(string data)
        {

        }
    }
}
