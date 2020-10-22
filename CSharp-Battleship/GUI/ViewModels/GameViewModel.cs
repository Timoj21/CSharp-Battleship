﻿using GalaSoft.MvvmLight.Command;
using GUI.Models;
using GUI.Utils;
using GUI.ViewModels.Commands;
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

        public ParameterCommand ParameterCommand { get; set; }

        private int tempTimerP1 { get; set; }
        private int tempTimerp2 { get; set; }


        public GameViewModel(MainViewModel mainViewModel, Client client)
        {
            this.tempTimerP1 = 1;
            this.tempTimerp2 = 1;
            this.MainViewModel = mainViewModel;
            this.client = client;
            battlelogTextBlock = "Welcome to Battleship\n";

            this.client.OnGameStateChangeReceived += Client_OnGameStateChangeReceived;
            this.client.OnHitMissReceived += Client_OnHitMissReceived;

            this.client.OnReadyUpReceived += Client_OnReadyUpReceived;
            this.client.OnBattlelogReceived += Client_OnBattlelogReceived;
            this.client.OnAttackReceived += Client_OnAttackReceived;


            gridButtonCommand = new RelayCommand<string>((s) => GridButtonCommandHandler(s));

            //gridButtonCommand = new RelayCommand(() =>
            //{
            //    GridButtonCommandHandler();
            //});

            //gridButtonCommand = new RelayCommand<object>(GridButtonCommandHandler);

            readyUpButtonCommand = new RelayCommand(() =>
            {
                ReadyUpButtonCommandHandler();
            });

        }

        public void ParameterMethod(string cell)
        {

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
                case "Playing":
                    {
                        if (MainViewModel.player.isPlayer1 && MainViewModel.player.Turn)
                        {
                            battlelogTextBlock += "Its player 1 his turn\n";
                        } else if (!MainViewModel.player.isPlayer1 && MainViewModel.player.Turn)
                        {
                            battlelogTextBlock += "Its player 2 his turn\n";
                        }
                        break;
                    }
                case "Ended":
                    {

                        break;
                    }
            }
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
                //this.client.SendReadyUp(MainViewModel.player.isPlayer1, MainViewModel.player.boatPositions);
            }
        }

        private void GridButtonCommandHandler(object parameter)
        {
            var str = parameter as string;


            if (MainViewModel.player.isPlayer1)
            {
                this.client.SendCell(MainViewModel.player.isPlayer1, str);
                this.tempTimerP1 += 1;
            } else
            {
                this.client.SendCell(MainViewModel.player.isPlayer1, str);
                this.tempTimerp2 += 1;
            }

            //if(MainViewModel.player.boatPositions[2] == null)
            //{
            //    MainViewModel.player.boatPositions[0] = "A1";
            //    MainViewModel.player.boatPositions[1] = "A2";
            //    MainViewModel.player.boatPositions[2] = "A3";
            //}

            //if (MainViewModel.player.Turn)
            //{
            //    this.client.SendAttack(MainViewModel.player.isPlayer1, "A1");
            //}
        }
    }
}
