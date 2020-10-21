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
        private Client client;
        public string battlelogTextBlock { get; set; }

        public ICommand gridButtonCommand { get; set; }

        public ICommand readyUpButtonCommand { get; set; }


        public GameViewModel(MainViewModel mainViewModel, Client client)
        {
            this.MainViewModel = mainViewModel;
            this.client = client;
            battlelogTextBlock = "Welcome to Battleship\nChoose 3 cells and then ready up\n";
            this.client.OnReadyUpReceived += Client_OnReadyUpReceived;
            this.client.OnBattlelogReceived += Client_OnBattlelogReceived;
            this.client.OnAttackReceived += Client_OnAttackReceived;

            gridButtonCommand = new RelayCommand(() =>
            {
                GridButtonCommandHandler(gridButtonCommand);
            });

            readyUpButtonCommand = new RelayCommand(() =>
            {
                ReadyUpButtonCommandHandler();
            });

        }

        private void Client_OnAttackReceived(bool hit)
        {
            if (MainViewModel.player.Turn)
            {
                this.client.SendBattlelogMessage("true");
            } else
            {
                this.client.SendBattlelogMessage("false");
            }
        }

        private void Client_OnBattlelogReceived(string message)
        {
            this.battlelogTextBlock += message;
        }

        private void Client_OnReadyUpReceived(bool state)
        {
            if (state)
            {
                this.client.SendBattlelogMessage("Its player 1 its turn\n");
            }

            if (MainViewModel.player.isPlayer1)
            {
                MainViewModel.player.Turn = true;
            }
            else
            {
                MainViewModel.player.Turn = false;
            }
        }

        private void ReadyUpButtonCommandHandler()
        {
            if (MainViewModel.player.boatPositions[2] != null)
            {
                this.client.SendReadyUp(MainViewModel.player.isPlayer1, MainViewModel.player.boatPositions);
            }
        }

        private void GridButtonCommandHandler(object parameter)
        {
            //TODO dit nog aanpassen wanneer de buttons goed werken
            if(MainViewModel.player.boatPositions[2] == null)
            {
                MainViewModel.player.boatPositions[0] = "A1";
                MainViewModel.player.boatPositions[1] = "A2";
                MainViewModel.player.boatPositions[2] = "A3";
            }

            if (MainViewModel.player.Turn)
            {
                this.client.SendAttack(MainViewModel.player.isPlayer1, "A1");
            }
        }
    }
}
