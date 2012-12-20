using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnifiToEM.Models;

namespace UnifiToEM.IO
{
    internal class CategoryReader : CategoryFile
    {
        internal List<Category> ReadCategories()
        {
            using (XmlReader reader = new XmlNodeReader(Document.DocumentElement))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Category>), new XmlRootAttribute(Categories));
                List<Category> categories = serializer.Deserialize(reader) as List<Category>;
                categories.Sort((category1, category2) => string.Compare(category1.Name, category2.Name));
                return categories;
            }
        }
    }
}
