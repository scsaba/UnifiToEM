using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnifiToEM.Models;

namespace UnifiToEM.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private Category category;
        private ObservableCollection<StringItem> matchingPatterns;

        public Category Category
        {
            get
            {
                return category;
            }
            set
            {
                this.category = value;
                List<StringItem> strings = new List<StringItem>();
                foreach (StringItem pattern in value.MatchingPatterns)
                {
                    pattern.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == "Value")
                        {
                            StringItem item = (StringItem)sender;
                            category.MatchingPatterns[category.MatchingPatterns.IndexOf(item.OldValue)] = item.Value;
                        }
                    };
                    strings.Add(pattern);
                }
                this.matchingPatterns = new ObservableCollection<StringItem>(strings);
                value.PatternsChanged += (change, pattern) =>
                    {
                        StringItem patternItem = (StringItem)pattern;
                        if (change == PatternsChanged.Added)
                        {
                            this.matchingPatterns.Add(patternItem);
                        }
                        else if (change == PatternsChanged.Removed)
                        {
                            if (this.matchingPatterns.Contains(patternItem))
                            {
                                this.matchingPatterns.Remove(patternItem);
                            }
                        }
                    };
                
            }
        }
        
        public String Name
        {
            get
            {
                return category.Name;
            }
            set
            {
                if (category.Name != value)
                {
                    category.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public ObservableCollection<StringItem> MatchingPatterns
        {
            get
            {
                return matchingPatterns;
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

        private ICommand deletePatternCommand;
        public ICommand DeletePatternCommand
        {
            get
            {
                if (deletePatternCommand == null)
                {
                    deletePatternCommand = new RelayCommand(param => DeletePattern(param));
                }
                return deletePatternCommand;
            }
        }

        private void DeletePattern(object param)
        {
            category.RemovePattern((StringItem)param);
        }

        private ICommand addPatternCommand;
        public ICommand AddPatternCommand
        {
            get
            {
                if (addPatternCommand == null)
                {
                    addPatternCommand = new RelayCommand(param => AddPattern(param));
                }
                return addPatternCommand;
            }
        }

        private void AddPattern(object param)
        {
            category.AddPattern(param as String);
        }
    }
}
