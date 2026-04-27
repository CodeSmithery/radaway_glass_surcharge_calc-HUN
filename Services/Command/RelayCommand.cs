using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace radaway_surcharge_calc_HUN.Services.Command
{
    public class RelayCommand : ICommand //ICommand interface ami az osztályt valósítja meg, parancshasználathoz
    {
        private readonly Action _execute; 
        private readonly Func<bool> _canExecute = () => false; // Alapaból soha nem kattintható a gomb, csak explicit jelzésnél válik azzá

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute; //A konstruktorban átadjuk a végrehajtandó műveletet és egy opcionális feltételt, hogy mikor lehet végrehajtani a parancsot
            _canExecute = canExecute; 
        }

        public bool CanExecute(object parameter) => _canExecute(); 

        public void Execute(object parameter) => _execute();

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

}
