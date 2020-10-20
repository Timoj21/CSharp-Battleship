using GUI.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.ViewModels
{
    public class GameViewModel : ObserverableObject
    {
        private MainViewModel MainViewModel { get; set; }
        public string battlelogTextBlock { get; set; }

        public GameViewModel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;

        }
    }
}
