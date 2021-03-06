﻿using GalaSoft.MvvmLight.Command;
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

        public GameViewModel(MainViewModel mainViewModel, Client client)
        {
            this.MainViewModel = mainViewModel;
            this.client = client;
            battlelogTextBlock = "Welcome to Battleship\n";

            this.client.OnGameStateChangeReceived += Client_OnGameStateChangeReceived;
            this.client.OnHitMissReceived += Client_OnHitMissReceived;
  
            gridButtonCommand = new RelayCommand<string>((s) => GridButtonCommandHandler(s));
        }

        private void Client_OnHitMissReceived(string cell, bool hit)
        {
            if (hit)
            {
                battlelogTextBlock += "HIT\n";
            } else
            {
                battlelogTextBlock += "MISS\n";
            }
        }

        private void Client_OnGameStateChangeReceived(string gameState)
        {
            switch (gameState)
            {
                case "Waiting":
                    {
                        battlelogTextBlock += "Waiting for player 2\n";
                        break;
                    }
                case "ChooseCells":
                    {
                        battlelogTextBlock += "Choose 3 cells and click on ready\n";
                        break;
                    }
                case "Player1Turn":
                    {
                        battlelogTextBlock += "Its player 1 his turn\n";
                        if (MainViewModel.player.isPlayer1)
                        {
                            MainViewModel.player.Turn = true;
                        } else
                        {
                            MainViewModel.player.Turn = false;
                        }
                        break;
                    }
                case "Player2Turn":
                    {
                        battlelogTextBlock += "Its player 2 his turn\n";
                        if (MainViewModel.player.isPlayer1)
                        {
                            MainViewModel.player.Turn = false;
                        }
                        else
                        {
                            MainViewModel.player.Turn = true;
                        }
                        break;
                    }
                case "Ended":
                    {
                        if(MainViewModel.player.isPlayer1 && MainViewModel.player.Turn)
                        {
                            battlelogTextBlock = "Player 1 won!\n";
                        } else if(!MainViewModel.player.isPlayer1 && MainViewModel.player.Turn)
                        {
                            battlelogTextBlock = "Player 2 won!\n";
                        } else if (MainViewModel.player.isPlayer1 && !MainViewModel.player.Turn)
                        {
                            battlelogTextBlock = "Player 2 won!\n";
                        } else if (!MainViewModel.player.isPlayer1 && !MainViewModel.player.Turn)
                        {
                            battlelogTextBlock = "Player 1 won!\n";
                        }

                        MainViewModel.player.Turn = false;
                        break;
                    }
            }
        }

        private void GridButtonCommandHandler(object parameter)
        {
            if (MainViewModel.player.Turn)
            {
                var str = parameter as string;
                this.client.SendCell(MainViewModel.player.isPlayer1, str);
            }
        }
    }
}
