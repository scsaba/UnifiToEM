using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnifiToEM.Models;

namespace UnifiToEM.IO
{
    internal class CIBTxtFileImport : IFileImport
    {
        const string TIME_PERIOD = "IDŐSZAK:";
        const string TRANSACTIONS_START = "NYITÓ EGYENLEG:";
        const string TRANSACTIONS_END = "ÖSSZESEN FORGALOM";

        const string OUTGOING_TRANSFER = "Bankon belüli eseti utalás";
        const string INCOMING_TRANSFER = "Bejövő giro jóváírás";
        //const string PAYING_BY_CARD = "Bankkártyával történő vásárlás";
        const string TRANSFERING_CASH_TO_ACCOUNT = "Készpénz befizetés";
        const string INTEREST = "KIFIZETETT KAMAT";
        const string ACCOUNT_FEE = "Jutalék";

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

                    if (compareInfo.IndexOf(line, TRANSACTIONS_START) > 0)
                    {
                        List<String> transactionText = new List<String>();
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (compareInfo.IndexOf(line, TRANSACTIONS_END) > 0)
                            {
                                break;
                            }

                            if (!String.IsNullOrWhiteSpace(line))
                            {
                                transactionText.Add(line);
                            }
                            else
                            {
                                AddTransaction(transactionText);
                                transactionText = new List<String>();
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(TimePeriod) && ImportedTransactions != null && ImportedTransactions.Count() > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddTransaction(List<string> transactionText)
        {
            if (transactionText.Count == 0)
            {
                return;
            }

            string regex = @"(?<date>[0-9]{4}\.[0-9]{2}\.[0-9]{2}\.)\s+(?<transactionType>[\s\S]+);([\s\S]+?)((?<amount>[0-9\.\,-]+)[\s]+([0-9\.\,-]+)$)";
            Match m = Regex.Match(transactionText[0].Trim(), regex);

            string date = m.Groups["date"].Value;
            string transactionType = m.Groups["transactionType"].Value;
            string amountString = m.Groups["amount"].Value;

            // first line should start with date
            if (String.IsNullOrEmpty(date))
            {
                return;
            }

            CultureInfo culture = new CultureInfo("hu-HU");
            DateTime transactionDate;
            if (!DateTime.TryParse(date, culture, DateTimeStyles.NoCurrentDateDefault, out transactionDate))
            {
                return;
            }

            NumberFormatInfo numberformatInfo = new NumberFormatInfo();
            numberformatInfo.NumberDecimalSeparator = ",";
            numberformatInfo.NumberGroupSeparator = ".";

            double amount = double.Parse(amountString, numberformatInfo);

            Transaction transaction = CreateTransaction(transactionDate, transactionType, amount, transactionText);
            importedTransactions.Add(transaction);
        }

        private Transaction CreateTransaction(DateTime transactionDate, string transactionType, double amount, List<String> transactionInfos)
        {
            string payee_item_description = transactionType;
            Category category = null;
            
            //string transactionStatus = "Uncleared";
            string remarks = FormatRemarks(transactionInfos);

            switch (transactionType)
            {
                case OUTGOING_TRANSFER:
                case INCOMING_TRANSFER:
                //case PAYING_BY_CARD:
                    payee_item_description = transactionInfos[2].Trim();
                    break;
                case ACCOUNT_FEE:
                    payee_item_description = "Bankszámla díj";
                    break;
            }
            return new Transaction() { Description = payee_item_description, Category = category, Amount = amount, /*Status = transactionStatus, */Date = transactionDate, Remarks = remarks };
        }

        private string FormatRemarks(List<string> transactionInfos)
        {
            StringBuilder sb = new StringBuilder();
            transactionInfos.ForEach(s => sb.AppendLine(Regex.Replace(s.Trim(), @"\s+", " ")));
            return sb.ToString();
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
