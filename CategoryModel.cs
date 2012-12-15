using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace UnifiToEM
{
    [XmlType("Pattern")]
    public class StringItem : INotifyPropertyChanged
    {
        private string stringValue;

        [XmlText]
        public string Value 
        {
            get
            {
                return stringValue;
            }
            set
            {
                if (stringValue != value)
                {
                    stringValue = value;
                    OnPropertyChanged("Value");
                }
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

    [XmlType("Category")]
    public class CategoryModel : INotifyPropertyChanged
    {
        private string name;
        private ObservableCollection<StringItem> matchingPatterns;

        public String Name 
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        [XmlArray]
        [XmlArrayItem(typeof(StringItem), ElementName="Pattern")]
        public ObservableCollection<StringItem> MatchingPatterns 
        {
            get
            {
                return matchingPatterns;
            }
            set
            {
                if (matchingPatterns != value)
                {
                    matchingPatterns = value;
                    OnPropertyChanged("MatchingPatterns");
                }
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
            matchingPatterns.Remove(param as StringItem);
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
            matchingPatterns.Add(new StringItem() { Value = param as string });
        }
    }
}
