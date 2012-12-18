using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiToEM.ViewModels
{
    public class StringItem : INotifyPropertyChanged
    {
        private string stringValue;

        public StringItem(string value)
        {
            this.stringValue = value;
        }

        internal String OldValue
        {
            get;
            private set;
        }

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
                    OldValue = stringValue;
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

        public static explicit operator StringItem(string str)
        {
            StringItem item = new StringItem(str);
            return item;
        }

        public static implicit operator String(StringItem stringItem)
        {
            return stringItem.Value;
        }

        public override bool Equals(object obj)
        {
            StringItem other = obj as StringItem;
            if (other == null)
            {
                return base.Equals(obj);
            }
            return stringValue.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return stringValue.GetHashCode();
        }
    }
}
