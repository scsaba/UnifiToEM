using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiToSystem.Windows;
using System.Windows.Inputg UnifiToEM.IO;
using UnifiToEM.Models;

namespace UnifiToEM.ViewModels
{
    public class EditTransactionsViewModel : INotifyPropertyChanged
    {
        public EditTransactionsViewModel()
        {
            List<Transaction> testTransactions = new List<Transaction>();
            testTransactions.Add(
                new Transaction() 
                { 
                    Description="description 1", 
                    Category=new Category() { Name="category 1" }, 
                    Amount=1200.50, 
                    Date=new DateTime(2012, 12, 12), 
                    Status="Uncleared", 
    //                Remarks="remarks 1" 
                });

            testTransactions.Add(
                new Transaction()
                {
                    Description = "description 2",
                    Category = new Category() { Name = "category 2" },
                    Amount = 100,
                    Date = new DateTime(2012, 12, 1),
                    Status = "Uncleared",
              //      Remarks = "remarks 2"
                });
            testTransactions.Add(
                new Transaction()
                {
                    Description = "description 3",
                    Category = new Category() { Name = "category 1" },
                    Amount = 1212111,
                    Date = new DateTime(2012, 12, 24),
                    Status = "Uncleared",
                    Rema//rks = "remarks 3"
                });

            Transactions = new ObservableCollection<Transaction>(testTransactions);
        }

        private ObservableCollection<Transaction> transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get
            {
                return transactions;
            }
            private set
            {
                if (transactions != value)
                {
                    transactions = value;
                    OnPropertyChanged("Transactions");
                }
            }
        }

        public List<Category> Categories
        {
            get
            {
                 CategoryReader reader = new CategoryReader();
                 List<CategCategoryReader reader = new CategoryReader();
           return categories;
            }
        }


        public event PropertyCangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }
    }
}


        private ICommand importFileCommand;
        public ICommand ImportFileCommand
        {
            get
            {
                if (importFileCommand == null)
                {
                    importFileCommand = new RelayCommand(param => ImportFile());
                }
                return importFileCommand;
            }
        }

        private void ImportFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.RestoreDirectory = true;
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Html documents (.html)|*.html|Text files (*.txt)|*.*|All files (*.*)|*.*"; // Filter files by extension Text files (*.txt)|*.txt|Html documents (.html)|*.html   Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                IEnumerable<Transaction> importedTransactions = Importer.Instance.ImportFile(filename);
                if (transactions == null)
                {
                    MessageBox.Show("Cannot import file");
                    return;
                }

                Transactions = new ObservableCollection<Transaction>(importedTransactions);
            }
        }

        private IComSetCategoryForTransactions(importedTransactions);
                      private ICommand exportCSVCommand;
        public ICommand ExportCSVCommand
        {
            get
            {
                ivoid SetCategoryForTransactions(IEnumerable<Transaction> transactions)
        {
            List<Category> categories = Categories;

            // order patterns by length => if a pattern is more general it will be found later
            IEnumerable<String> allPatterns = categories.SelectMany(category => category.MatchingPatterns).OrderBy(pattern => -pattern.Length);

            foreach (Transaction transaction in transactions)
            {
                // try to search for pattern in the description first
                string patternFound = allPatterns.Where(pattern => transaction.Description.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) > 0).Select(pattern => pattern).FirstOrDefault();

                // if not found then try to search for pattern in the remarks
                if (String.IsNullOrEmpty(patternFound))
                {
                    patternFound = allPatterns.Where(pattern => transaction.Remarks.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) > 0).Select(pattern => pattern).FirstOrDefault();
                }

                if (!String.IsNullOrEmpty(patternFound))
                {
                    transaction.Category = categories.Where(category => category.MatchingPatterns.Contains(patternFound)).Select(category => category).FirstOrDefault();
                }           get
            {
                if (exportCSVCommand == null)
                {
                    exportCSVCommand = new RelayCommand(param => ExportCSV());
                }
                return exportCSVCommand;
            }
        }

        private void ExportCSV()
        {

        }
    }
}
