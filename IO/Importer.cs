using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiToEM.Models;

namespace UnifiToEM.IO
{
    internal class Importer
    {
        private Importer()
        {
            FileImporters = new List<IFileImport>();
            FileImporters.Add(new HtmlFileImport());
        }

        private static Importer instance;
        internal static Importer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Importer();
                }
                return instance;
            }
        }

        private List<IFileImport> FileImporters { get; set; }

        internal IEnumerable<Transaction> ImportFile(string fileName)
        {
            string extension = Path.GetExtension(fileName);

            foreach (IFileImport fileImport in FileImporters)
            {
                if (!extension.Equals(fileImport.FileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                bool result = fileImport.Import(fileName);
                if (!result)
                {
                    continue;
                }

                return fileImport.ImportedTransactions;
            }

            return null;
        }
    }
}
