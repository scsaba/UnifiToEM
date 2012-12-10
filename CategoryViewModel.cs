using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace UnifiToEM
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private CategoryModel currentCategory;
        private ICommand readCategoryCommand;
        private ICommand saveCategoryCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public CategoryModel CurrentCategory
        {
            get
            {
                return currentCategory;
            }
            set
            {
                if (currentCategory != value)
                {
                    currentCategory = value;
                    OnPropertyChanged("CurrentCategory");
                }
            }
        }

        public ICommand ReadCategoryCommand
        {
            get
            {
                if (readCategoryCommand == null)
                {
                    readCategoryCommand = new RelayCommand(param => ReadCategory());
                }
                return readCategoryCommand;
            }
        }

        public ICommand SaveCategoryCommand
        {
            get
            {
                if (saveCategoryCommand == null)
                {
                    saveCategoryCommand = new RelayCommand(param => SaveCategory());
                }
                return saveCategoryCommand;
            }
        }

        private void ReadCategory()
        {
            XmlDocument categoriesXml = new XmlDocument();
            categoriesXml.Load("Categories.xml");

            Categories = CategoryReader.ReadCategories(categoriesXml.DocumentElement);

            CategoryModel m = new CategoryModel();
            CurrentCategory = m;
        }

        private List<Category> categories;
        public List<Category> Categories 
        {
            get
            {
                return categories;
            }
            set
            {
                if (categories != value)
                {
                    categories = value;
                    OnPropertyChanged("Categories");
                }
            }
        }

        private void SaveCategory()
        {
        }

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
