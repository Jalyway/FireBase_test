using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Standard.ViewModels
{
    public class InvoiceVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Key { get; set; }
        public string InvoiceNo { get; set; }
        public string Title { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string Memo { get; set; }
    }
}
