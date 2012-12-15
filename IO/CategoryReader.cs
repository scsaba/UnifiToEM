using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnifiToEM.Models;

namespace UnifiToEM.IO
{
    internal class CategoryReader : CategoryFile
    {
        internal List<CategoryModel> ReadCategories()
        {
            using (XmlReader reader = new XmlNodeReader(Document.DocumentElement))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<CategoryModel>), new XmlRootAttribute(Categories));
                List<CategoryModel> categories = serializer.Deserialize(reader) as List<CategoryModel>;
                return categories;
            }
        }
    }
}
