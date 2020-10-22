using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace GUI.ViewModels.Commands
{
    public class ParameterCommand : ICommand
    {
        public GameViewModel ViewModel { get; set; }

        public ParameterCommand(GameViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if(parameter != null)
            {
                var s = parameter as string;
                if (string.IsNullOrEmpty(s))
                    return false;

                return true;
            }
            return false;
        }

        public void Execute(object parameter)
        {
            this.ViewModel.ParameterMethod(parameter as string);
        }
    }
}
