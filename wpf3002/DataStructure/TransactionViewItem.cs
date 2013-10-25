using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf3002.DataStructure
{
    public class TransactionViewItem
    {
        public String name{get;set;}
        public String barcode { get; set; }
        public String price { get; set; }
        public String quantity { get; set; }
        public TransactionViewItem(String _name, String _barcode, String _price, String _quantity)
        {
            name = _name;
            price = _price;
            barcode = _barcode;
            quantity = _quantity;
        }
    }
}
