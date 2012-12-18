using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UnifiToEM.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private CategoryListViewModel categoryListViewModel;
        private EditTransactionsViewModel editTransactionsViewModel;

        public MainWindowViewModel()
        {
            categoryListViewModel = new CategoryListViewModel();
            editTransactionsViewModel = new EditTransactionsViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }

        }

        private object activeViewModel;
        public object ActiveViewModel
        {
            get
            {
                return activeViewModel;
            }
            set
            {
                if (activeViewModel != value)
                {
                    activeViewModel = value;
                    OnPropertyChanged("ActiveViewModel");
                }
            }
        }

        private ICommand editCategoriesCommand;
        public ICommand EditCategoriesCommand
        {
            get
            {
                if (editCategoriesCommand == null)
                {
                    editCategoriesCommand = new RelayCommand(param => EditCategories(), param => CanEditCategories());
                }
                return editCategoriesCommand;
            }
        }
        
        private bool CanEditCategories()
        {
            return activeViewModel != categoryListViewModel;
        }

        private void EditCategories()
        {
            ActiveViewModel = categoryListViewModel;            
        }

        private ICommand editTransactionsCommand;
        public ICommand EditTransactionsCommand
        {
            get
            {
                if (editTransactionsCommand == null)
                {
                    editTransactionsCommand = new RelayCommand(param => EditTransactions(), param => CanEditTransactions());
                }
                return editTransactionsCommand;
            }
        }

        private bool CanEditTransactions()
        {
            return activeViewModel != editTransactionsViewModel;
        }

        private void EditTransactions()
        {
            ActiveViewModel = editTransactionsViewModel;
        }
    }
}
