using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiToEM.Models;

namespace UnifiToEM.IO
{
    internal interface IFileImport
    {
        bool Import(String fileName);

        string FileExtension { get; }

        IEnumerable<Transaction> ImportedTransactions { get; }

        string TimePeriod { get; }
    }
}
