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
    public enum PatternsChanged
    {
        None,
        Added,
        Removed
    }

    [XmlType("Category")]
    public class Category
    {
        private string name;
        private readonly List<String> matchingPatterns = new List<string>();

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
                }
            }
        }

        [XmlArray]
        [XmlArrayItem(typeof(String), ElementName="Pattern")]
        public List<String> MatchingPatterns 
        {
            get
            {
                return matchingPatterns;
            }  }

        public void AddPattern(string pattern)
        {
            if (matchingPatterns.Contains(pattern))
            {
                return;
            }

            matchingPatterns.Add(pattern);
            RaisePatternsChanges(Models.PatternsChanged.Added, pattern);
        }

        public void RemovePattern(string pattern)
        {
            if (matchingPatterns.Contains(pattern))
            {
                matchingPatterns.Remove(pattern);
                RaisePatternsChanges(Models.PatternsChanged.Removed, pattern);
            }
        }

        public event Action<PatternsChanged, String> PatternsChanged;

        void RaisePatternsChanges(PatternsChanged changed, string pattern)
        {
            if (PatternsChanged != null)
            {
                PatternsChanged(changed, pattern);
            }
        }
    }
}

    }
}
