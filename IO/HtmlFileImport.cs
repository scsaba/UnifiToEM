using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using UnifiToEM.Models;

namespace UnifiToEM.IO
{
    internal class HtmlFileImport : IFileImport
    {
        const string OUTGOING_TRANSFER = "Eseti GIRO átutalás NetBank";
        const string INCOMING_TRANSFER = "Bejövő HUF átutalás (Giro)";
        const string PAYING_BY_CARD = "Bankkártyával történő vásárlás";
        const string TRANSFERING_CASH_TO_ACCOUNT = "Készpénz befizetés ügyfélszámlára";
        const string BANKCARD_YEARLY_FEE = "Bankkártya éves díja";
        const string DEPOSIT_LOCKUP = "Betét lekötés";
        const string DEPOSIT_BREAKUP = "Lekötött betét feltörése";
        const string INTEREST = "Bankszámla betéti kamat fizetése";
        const string INTEREST_TAX = "Kamat forrásadó elszámolása";

        private IList<Transaction> importedTransactions;

        public bool Import(String fileName)
        {
            importedTransactions = new Liststring content = String.Empty;
            using (TextReader reader = new StreamReader(fileName, Encoding.GetEncoding("windows-1250")))
            {
                content = reader.ReadToEnd();
            }
ons = new List<Transaction>();

            HtmlDocument document = new HtmlDocument();
            document.Load(fileName);

            foreach (Html(contentnode in document.DocumentNode.SelectNodes("//table"))
            {
                HtmlNode captionNode = node.SelectSingleNode("caption");
                if (captionNode.InnerText.Equals("Keres&eacute;si felt&eacute;telek", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!GetTimePeriod(node))
                    {
                        return false;
                    }
                }
                else if (captionNode.InnerText.Equals("A keres&eacute;s eredm&eacute;nye"))
                {
                    if (!GetTransactions(node))
                    {
                        return false;
                    }
                }
            }

            if (!String.IsNullOrEmpty(TimePeriod) && ImportedTransactions != null && ImportedTransactions.Count() > 0)
            {
                return true;
            }

            return false;
        }

        private bool GetTimePeriod(HtmlNode tableNode)
        {
            // the time period is in the second row, second cell
            HtmlNode lastRow = tableNode.ChildNodes.LastOrDefault(child => child.Name == "tr");
            if (lastRow == null)
            {
                return false;
            }

            HtmlNode lastCell = lastRow.ChildNodes.LastOrDefault(child => child.Name == "td");
            if (lastCell == null)
            {
                return false;
            }

            TimePeriod = System.Text.RegularExpressions.Regex.Replace(lastCell.InnerText, @"\s+", " ");
            return true;
        }

        private bool GetTransactions(HtmlNode tableNode)
        {
            foreach (HtmlNode row in tableNode.SelectNodes("tr"))
            {
                AddTransaction(row);
            }
            return true;
        }

        public IEnumerable<Models.Transaction> ImportedTransactions
        {
            get { return importedTransactions; }
        }

        public string FileExtension
        {
            get { return ".html"; }
        }

        public string TimePeriod
        {
            get;
            private set;
        }

        private string HtmlDecode(string input)
        {
            return WebUtility.HtmlDecode(input).Trim().Replace("Ĺ‘", "ő").Replace("Ĺ±", "ű");
        }

        private void AddTransaction(HtmlNode row)
        {
            HtmlNodeCollection cells = row.SelectNodes("td");
            string date = cells[0].InnerText;
            if (String.IsNullOrEmpty(date))
            {
                // if the first cell is empty then it's not a transaction
                return;
            }

            // Tranzakci&oacute; d&aacute;tuma
            // Tranzakci&oacute; t&iacute;pusa
            // Kedvezm&eacute;nyezett banksz&aacute;mla
            // Partner banksz&aacute;mla
            // Partner neve
            // &Eacute;rt&eacute;knap
            // K&ouml;zlem&eacute;ny
            // Le&iacute;r&aacute;s
            // J&oacute;v&aacute;&iacute;r&aacute;s
            // Terhel&eacute;s
            // Egyenleg

            string transactionType = HtmlDecode(cells[1].InnerText);
            string toAccount = cells[2].InnerText;
            string partnerAccount = cells[3].InnerText;
            string partnerName = HtmlDecode(cells[4].InnerText);
            string executionDate = cells[5].InnerText;
            string comments = HtmlDecode(cells[6].InnerText);
            string description = HtmlDecode(cells[7].InnerText);
            string deposit = cells[8].InnerText.Replace("HUF", String.Empty).Replace(" ", String.Empty);
            string withdraw = cells[9].InnerText.Replace("HUF", String.Empty).Replace(" ", String.Empty);

            Transaction transaction = CreateTransaction(date, transactionType, toAccount, partnerAccount, partnerName, executionDate, comments, description, deposit, withdraw);
            importedTransactions.Add(transaction);
        }

        private Transaction CreateTransaction(string date, string transactionType, string toAccount, string partnerAccount, string partnerName, string executionDate, string comments, string description, string deposit, string withdraw)
        {
            string payee_item_description = transactionType;
            Category category = null;
            double amount = String.IsNullOrEmpty(deposit) ? -double.Parse(withdraw) : double.Parse(deposit);
            //string transactionStatus = "Uncleared";
            DateTime transactionDate = DateTime.Parse(date, CultureInfo.CreateSpecificCulture("hu-HU"));
            string remarks = FormatRemarks(transactionType, partnerAccount, partnerName, executionDate, comments, description);                                        

            switch (transactionType)
            {
                case OUTGOING_TRANSFER:
                case INCOMING_TRANSFER:
                case PAYING_BY_CARD:
                    payee_item_description = partnerName;
                    break;
            }
            return new Transaction() { Description = payee_item_description, Category = category, Amount = amount, /*Status = transactionStatus, */Date = transactionDate, Remarks = remarks };
        }

        private string FormatRemarks(string transactionType, string partnerAccount, string partnerName, string executionDate, string comments, string description)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Tranzakció típusa: " + transactionType);
            if (!String.IsNullOrEmpty(partnerName))
            {
                sb.AppendLine("Partner neve: " + partnerName);
            }
            if (!String.IsNullOrEmpty(partnerAccount))
            {
                sb.AppendLine("Partner bankszámlaszáma: " + partnerAccount);
            }
            if (!String.IsNullOrEmpty(executionDate))
            {
                sb.AppendLine("Értéknap: " + executionDate);
            }
            if (!String.IsNullOrEmpty(comments))
            {
                sb.AppendLine("Közlemény: " + comments);
            }
            if (!String.IsNullOrEmpty(description))
            {
                sb.AppendLine("Leírás: " + description);
            }
            return sb.ToString();
        }
    }
}
