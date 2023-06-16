using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _3ShapeMainboardLSTester.Pages.ViewModels;
using System.Diagnostics;

namespace _3ShapeMainboardLSTester.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        public UpdateViewCommand()
        {
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter.ToString() == "Settings/Home")
            {
                if(viewModel.SelectedViewModel.GetType() == typeof(UserControl1ViewModel))
                {
                    viewModel.SelectedViewModel = new MainPageViewModel();
                }
                else
                {
                    viewModel.SelectedViewModel = new UserControl1ViewModel();
                }

            }
        }
    }
}
