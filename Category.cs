using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UnifiToEM
{
    public class Category
    {
        public String Name { get; set; }

        [XmlArray]
        [XmlArrayItem("Pattern")]
        public List<String> MatchingPatterns { get; set; }
    }
}
