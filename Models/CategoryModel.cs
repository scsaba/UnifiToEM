using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace UnifiToEM.Models
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
                    OnPropertyChanged("Value        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string ropertyName)
        {
            if (this.PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, e);
            }

        }

        private ICommand deletePatternCommandlic class CategoryModel : INotifyPropertyChanged
    {
        private string name;
        private ObservableCollection<Str
        private ObservableCollection<StringItem> matchingPatterns;
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
        [XmlArrayItem(t

        [XmlArray]entName="Pattern")]
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

        private ICommand deletePatternCommand

        private ICommand deletePatternCommand;
        public ICommand DeletePatternCommand
        {
            get
            {
                if (deletePatternCommand == null)
                {mand(param => DeletePattern(param));
                }
                return deletePatternC
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
