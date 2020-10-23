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
        public List<string> scoreboard { get; set; }
        private MainViewModel MainViewModel { get; set; }
        public ICommand backButtonCommand { get; set; }

        public Scoreboardviewmodel(MainViewModel mainViewModel)
        {
            this.MainViewModel = mainViewModel;
            this.MainViewModel.filewriteread.ReadFromFile();
            scoreboard = this.MainViewModel.filewriteread.outcomes;

            backButtonCommand = new RelayCommand(() =>
            {
                MainViewModel.SelectedViewModel = new StartViewModel(this.MainViewModel);
            });
        }

    }
}

