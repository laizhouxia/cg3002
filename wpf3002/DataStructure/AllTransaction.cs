using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf3002.DataStructure
{
    public class AllTransaction
    {
        public String barcode { get; set; }
        public String quantity { get; set; }
        public String price { get; set; }
        public DateTime date { get; set; }
        public AllTransaction(String _barcode, String _quantity, String _price, DateTime _date)
        {
            barcode = _barcode;
            quantity = _quantity;
            price = _price;
            date = _date;
        }
    }
}
