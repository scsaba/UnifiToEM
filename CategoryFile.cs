using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UnifiToEM
{
    internal class CategoryFile
    {
        private readonly XmlDocument document;
        private readonly string path;
        protected const string Categories = "Categories";

        internal CategoryFile() : this("Categories.xml")
        {
        }

        internal CategoryFile(string path)
        {
            this.path = path;
            document = new XmlDocument();

            if (File.Exists(path))
            {
                document.Load(path);
            }
        }

        protected XmlDocument Document
        {
            get
            {
                return document;
            }
        }

        protected string Path
        {
            get
            {
                return path;
            }
        }
    }
}
