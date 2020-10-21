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
        //private Client client;
        private MainViewModel MainViewModel { get; set; }
        public ICommand joinGameCommand { get; set; }

        public ICommand hostGameCommand { get; set; }

        public string Name { get; set; }



        public StartViewModel(MainViewModel mainViewModel)
        {
            //MainViewModel.client = new Client();
            //this.client = new Client();
            MainViewModel.client.OnDataReceived += Client_OnDataReceived;
            MainViewModel.client.OnInGameReceived += Client_OnInGameReceived;
            this.MainViewModel = mainViewModel;
            joinGameCommand = new RelayCommand(() =>
            {
                if (Name != null && Name.Length > 0)
                {
                    MainViewModel.client.SendJoinGame(Name);
                    //this.client.SendJoinGame(Name);
                    MainViewModel.player = new Player(Name, false);
                }
            });

            hostGameCommand = new RelayCommand(() =>
            {
                if (Name != null && Name.Length > 0)
                {
                    //this.client.SendHostGame(Name);
                    MainViewModel.client.SendHostGame(Name);
                    MainViewModel.player = new Player(Name, true);
                }
            });
        }

        private void Client_OnInGameReceived(bool state)
        {
            if (state)
            {
                MainViewModel.SelectedViewModel = new GameViewModel(this.MainViewModel);
            }
        }

        private void Client_OnDataReceived(string data)
        {

        }
    }
}
