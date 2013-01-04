using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifiToEM.Models;

namespace UnifiToEM.IO
{
    internal class CSVExporter
    {
        private const string PAYEE_ITEM_DESCRIPTION = "PAYEE_ITEM_DESC";
        private const string CATEGORY = "CATEGORY";
        private const string AMOUNT = "AMOUNT";
        private const string STATUS = "STATUS";
        private const string TRANSACTION_DATE = "TRAN_DATE";
        private const string REMARKS = "REMARKS";
        private const string FORMAT = "\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"";

        internal static void ExportTransactions(string path, IEnumerable<Transaction> transactions)
        {
            using (TextWriter sw = new StreamWriter(path, false))
            {
                sw.WriteLine(String.Format(FORMAT, PAYEE_ITEM_DESCRIPTION, CATEGORY, AMOUNT, STATUS, TRANSACTION_DATE, REMARKS));

                foreach (Transaction transaction in transactions)
                {
                    string description = Escape(transaction.Description);
                    string category = transaction.Category.Name;
                    double amount = transaction.Amount;
                    string status = "Uncleared";
                    string transactionDate = FormatDate(transaction.Date);
                    string remarks = Escape(FormatRemarks(transaction.Remarks));

                    sw.WriteLine(String.Format(FORMAT, description, category, amount, status, transactionDate, remarks));
                }

                sw.Flush();
            }
        }

        private static string FormatRemarks(string str)
        {
            return str.Replace(Environment.NewLine, "\t").Trim();
        }

        private static string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("d MMM yyyy");
        }

        private static string Escape(string str)
        {
            return str.Replace("\"", "\"\"");
        }
    }
}
