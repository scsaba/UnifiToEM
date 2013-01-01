using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiToEM.Models;

namespace UnifiToEM.IO
{
    internal class CIBTxtFileImport : IFileImport
    {
        const string TIME_PERIOD = "IDŐSZAK:";
        const string TRANSACTIONS_START = "NYITÓ EGYENLEG:";
        const string TRANSACTIONS_END = "ÖSSZESEN FORGALOM";

        private CompareInfo compareInfo = CompareInfo.GetCompareInfo("hu-HU");

        private IList<Transaction> importedTransactions;
        public bool Import(string fileName)
        {
            importedTransactions = new List<Transaction>();

            using (TextReader reader = new StreamReader(fileName, Encoding.GetEncoding("windows-1250")))
            {
                string line;
                string currentTransaction = String.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(TimePeriod))
                    {
                        if (compareInfo.IndexOf(line, TIME_PERIOD) > 0 && !GetTimePeriod(line))
                        {
                            return false;
                        }
                        else
                        {
                            continue;
                        }
                    }



                    if (compareInfo.IndexOf(line, TRANSACTIONS_END) > 0)
                    {
                        if (!String.IsNullOrEmpty(TimePeriod) && ImportedTransactions != null && ImportedTransactions.Count() > 0)
                        {
                            return true;
                        }
                    }
                }
            }


            //foreach (HtmlNode node in document.DocumentNode.SelectNodes("//table"))
            //{
            //    HtmlNode captionNode = node.SelectSingleNode("caption");
            //    if (captionNode.InnerText.Equals("Keres&eacute;si felt&eacute;telek", StringComparison.InvariantCultureIgnoreCase))
            //    {
            //        if (!GetTimePeriod(node))
            //        {
            //            return false;
            //        }
            //    }
            //    else if (captionNode.InnerText.Equals("A keres&eacute;s eredm&eacute;nye"))
            //    {
            //        if (!GetTransactions(node))
            //        {
            //            return false;
            //        }
            //    }
            //}

            //if (!String.IsNullOrEmpty(TimePeriod) && ImportedTransactions != null && ImportedTransactions.Count() > 0)
            //{
            //    return true;
            //}

            return false;
        }

        private bool GetTimePeriod(string line)
        {
            string[] parts = line.Split(' ');
            CultureInfo culture = new CultureInfo("hu-HU");
            DateTime dateTime;
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (DateTime.TryParse(part, culture, DateTimeStyles.NoCurrentDateDefault, out dateTime))
                {
                    TimePeriod = parts[i] + parts[i + 1] + parts[i + 2];
                    return true;
                }
            }
            return false;
        }

        public string FileExtension
        {
            get { return ".txt"; }
        }

        public IEnumerable<Models.Transaction> ImportedTransactions
        {
            get { return importedTransactions; }
        }

        public string TimePeriod
        {
            get;
            private set;
        }
    }
}
