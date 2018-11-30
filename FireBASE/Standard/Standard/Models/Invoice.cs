using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Models
{
    public class Invoice
    {
        public string InvoiceNo { get; set; }
        public string Title { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string Memo { get; set; }
    }
}
