using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiToEM.IO;
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
                    Remarks="remarks 1" 
                });

            testTransactions.Add(
                new Transaction()
                {
                    Description = "description 2",
                    Category = new Category() { Name = "category 2" },
                    Amount = 100,
                    Date = new DateTime(2012, 12, 1),
                    Status = "Uncleared",
                    Remarks = "remarks 2"
                });
            testTransactions.Add(
                new Transaction()
                {
                    Description = "description 3",
                    Category = new Category() { Name = "category 1" },
                    Amount = 1212111,
                    Date = new DateTime(2012, 12, 24),
                    Status = "Uncleared",
                    Remarks = "remarks 3"
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
                 List<Category> categories = reader.ReadCategories();
                 return categories;
            }
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
    }
}
