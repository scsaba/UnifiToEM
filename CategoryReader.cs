using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace UnifiToEM
{
    internal class CategoryReader
    {
        internal static List<CategoryModel> ReadCategories(XmlNode categoriesNode)
        {
            if (categoriesNode == null)
                throw new ArgumentNullException("categoriesNode");

            if (categoriesNode.Name != "Categories")
                throw new ArgumentException("categoriesNode");

            using (XmlReader reader = new XmlNodeReader(categoriesNode))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<CategoryModel>), new XmlRootAttribute("Categories"));
                List<CategoryModel> categories = serializer.Deserialize(reader) as List<CategoryModel>;
                return categories;
            }
        }
    }
}
