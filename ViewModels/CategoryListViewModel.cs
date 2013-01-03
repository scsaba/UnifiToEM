using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using UnifiToEM.IO;
using UnifiToEM.Models;

namespace UnifiToEM.ViewModels
{
    public class CategoryListViewModel : INotifyPropertyChanged
    {
        private CategoryViewModel currentCategory;
        private ICommand readCategoriesCommand;
        private ICommand saveCategoriesCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public CategoryViewModel CurrentCategory
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

        public ICommand ReadCategoriesCommand
        {
            get
            {
                if (readCategoriesCommand == null)
                {
                    readCategoriesCommand = new RelayCommand(param => ReadCategories());
                }
                return readCategoriesCommand;
            }
        }

        public ICommand SaveCategoriesCommand
        {
            get
            {
                if (saveCategoriesCommand == null)
                {
                    saveCategoriesCommand = new RelayCommand(param => SaveCategories());
                }
                return saveCategoriesCommand;
            }
        }

        private void ReadCategories()
        {
            CategoryReader reader = new CategoryReader();
            List<CategoryViewModel> categoryVMs = new List<CategoryViewModel>();
            List<Category> categories = reader.ReadCategories();
            foreach (Category category in categories)
            {
                CategoryViewModel model = new CategoryViewModel() { Category = category };
                categoryVMs.Add(model);
            }
            Categories = new ObservableCollection<CategoryViewModel>(categoryVMs);
            CurrentCategory = Categories.FirstOrDefault();
        }

        private ObservableCollection<CategoryViewModel> categories;
        public ObservableCollection<CategoryViewModel> Categories
        {
            get
            {
                return categories;
            }
            private set
            {
                if (categories != value)
                {
                    categories = value;
                    OnPropertyChanged("Categories");
                }
            }
        }

        private void SaveCategories()
        {
            CategoryWriter writer = new CategoryWriter();
            List<Category> categories = new List<Category>(Categories.Select(vm => vm.Category));
            writer.WriteCategories(categories);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }
        }

        private ICommand deleteCategoryCommand;
        public ICommand DeleteCategoryCommand
        {
            get
            {
                if (deleteCategoryCommand == null)
                {
                    deleteCategoryCommand = new RelayCommand(param => DeleteCategory(param));
                }
                return deleteCategoryCommand;
            }
        }

        private void DeleteCategory(object param)
        {
            Categories.Remove(param as CategoryViewModel);
        }

        private ICommand addCategoryCommand;
        public ICommand AddCategoryCommand
        {
            get
            {
                if (addCategoryCommand == null)
                {
                    addCategoryCommand = new RelayCommand(param => AddCategory(param));
                }
                return addCategoryCommand;
            }
        }

        private void AddCategory(object param)
        {
            if (Categories == null)
            {
                Categories = new ObservableCollection<CategoryViewModel>();
            }
            else if (Categories.Any(cvm => cvm.Category.Name == param as String))
            {
                return;
            }

            Category category = new Category() { Name = param as String };
            CategoryViewModel vm = new CategoryViewModel() { Category = category };
            Categories.Add(vm);
            CurrentCategory = vm;
        }
    }
}
