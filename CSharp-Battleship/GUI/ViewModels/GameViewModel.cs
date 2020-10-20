using GalaSoft.MvvmLight.Command;
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

        public GameViewModel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;

            gridButtonCommand = new RelayCommand(() =>
            {
                GridButtonCommandHandler(gridButtonCommand);
            });

        }

        private void GridButtonCommandHandler(object parameter)
        {
            switch(parameter.ToString())
            {
                case "A1":
                    {
                        battlelogTextBlock = "dit werkt";
                        break;
                    }
            }
        }
    }
}
