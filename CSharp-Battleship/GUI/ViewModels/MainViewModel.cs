﻿using GalaSoft.MvvmLight;
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
        public Player player { get; set; }
        public ObserverableObject SelectedViewModel { get; set; }

        public MainViewModel()
        {
            SelectedViewModel = new StartViewModel(this);
        }
    }
}
