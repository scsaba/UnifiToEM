﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UnifiToEM
{
    public class RelayCommand : ICommand
    {

        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameters)
        {
            return canExecute == null ? true : canExecute(parameters);
        }

        public void Execute(object parameters)
        {
            execute(parameters);
        }


        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
    }
}
