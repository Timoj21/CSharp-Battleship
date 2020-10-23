using GalaSoft.MvvmLight.Command;
using GUI.Models;
using GUI.Utils;
using System;
using System.Collections.Generic;
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
            this.client.OnJoinedGameReceived += Client_OnJoinedGameReceived;
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
                    MainViewModel.SelectedViewModel = new GameViewModel(this.MainViewModel, this.client);
                    MainViewModel.player = new Player(Name, true);
                }
            });

            scoresGameCommand = new RelayCommand(() =>
            {
                MainViewModel.SelectedViewModel = new Scoreboardviewmodel(this.MainViewModel);
            });
        }

        private void Client_OnJoinedGameReceived(bool state)
        {
            if (state)
            {
                MainViewModel.SelectedViewModel = new GameViewModel(this.MainViewModel, this.client);
                MainViewModel.player = new Player(Name, false);
            }
        }
    }
}
