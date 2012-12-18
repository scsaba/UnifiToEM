using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiToEM.Models
{
    public class Transaction
    {
        //1)PAYEE_ITEM_DESC
        //2)CATEGORY
        //3)AMOUNT
        //4)STATUS
        //5)TRAN_DATE
        //6)REMARKS

        public string Description { get; set; }
        
        public Category Category { get; set; }

        public double Amount { get; set; }

        public string Status { get; set; }

        public DateTime Date { get; set; }

        public string Remarks { get; set; }
    }
}
