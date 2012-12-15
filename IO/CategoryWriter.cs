using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnifiToEM.Models;

namespace UnifiToEM.IO
{
    internal class CategoryWriter : CategoryFile
    {
        internal CategoryWriter() : base() { }
        internal CategoryWriter(string path) : base(path) { }

        internal void WriteCategories(List<CategoryModel> categories)
        {
            if (categories == null)
                throw new ArgumentNullException("categories");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            using (XmlWriter writer = XmlTextWriter.Create(Path, settings))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                XmlSerializer serializer = new XmlSerializer(typeof(List<CategoryModel>), new XmlRootAttribute(Categories));
                serializer.Serialize(writer, categories, ns);
            }
        }
    }
}
