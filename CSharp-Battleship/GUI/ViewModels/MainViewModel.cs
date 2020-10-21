using GUI.Models;
using GUI.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GUI.ViewModels
{
    public class MainViewModel : ObserverableObject
    {
        public ObservableCollection<Player> players { get; set; } = new ObservableCollection<Player>();

        //public ObserverableObject player { get; set; } = new ObserverableObject();

        public Player player { get; set; }

        public Client client { get; set; }
        public ObserverableObject SelectedViewModel { get; set; }

        public MainViewModel()
        {
            SelectedViewModel = new StartViewModel(this);
            this.client = new Client();
        }
    }
}
