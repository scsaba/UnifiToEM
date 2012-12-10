using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnifiToEM
{
    public class CategoryModel : INotifyPropertyChanged
    {
        private string name;
        private List<String> matchingPatterns;

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
        
        public List<String> MatchingPatterns 
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
    }
}
