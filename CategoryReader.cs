using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace UnifiToEM
{
    internal class CategoryReader : CategoryFile
    {
        internal List<CategoryModel> ReadCategories()
        {
            using (XmlReader reader = new XmlNodeReader(Document.DocumentElement))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<CategoryModel>), new XmlRootAttribute("Categories);
                List<CategoryModel> categories = serializer.Deserialize(reader) as List<CategoryModel>;
                return categories;
            }
        }
    }
}
