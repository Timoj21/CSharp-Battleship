using GalaSoft.MvvmLight.Command;
using GUI.Models;
using GUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class GameViewModel : ObserverableObject
    {
        private MainViewModel MainViewModel { get; set; }
        public string battlelogTextBlock { get; set; }

        public ICommand gridButtonCommand { get; set; }

        public ICommand readyUpButtonCommand { get; set; }


        public GameViewModel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;
            battlelogTextBlock = "Welcome to Battleship\nChoose 3 cells and then ready up\n";
            MainViewModel.client.OnReadyUpReceived += Client_OnReadyUpReceived;

            gridButtonCommand = new RelayCommand(() =>
            {
                GridButtonCommandHandler(gridButtonCommand);
            });

            readyUpButtonCommand = new RelayCommand(() =>
            {
                ReadyUpButtonCommandHandler();
            });

        }

        private void Client_OnReadyUpReceived(bool state)
        {
            if (state)
            {
                if (MainViewModel.player.isPlayer1)
                {
                    battlelogTextBlock += "Its player 1 its turn";
                    MainViewModel.player.Turn = true;
                }
            }
        }

        private void ReadyUpButtonCommandHandler()
        {
            if(MainViewModel.player.boatPositions.Length == 3)
            {
                MainViewModel.client.SendReadyUp(MainViewModel.player.isPlayer1, MainViewModel.player.boatPositions);
            }
        }

        private void GridButtonCommandHandler(object parameter)
        {
            if(MainViewModel.player.boatPositions.Length != 3)
            {
                MainViewModel.player.boatPositions[0] = "A1";
                MainViewModel.player.boatPositions[0] = "A2";
                MainViewModel.player.boatPositions[0] = "A3";
            }

            if (MainViewModel.player.Turn)
            {
                MainViewModel.client.SendAttack(MainViewModel.player.isPlayer1, "A1");
            }
        }
    }
}
