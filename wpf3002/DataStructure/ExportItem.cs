using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf3002.DataStructure
{
    public class ExportItem
    {
        public String barcode { get; set; }

        public String quantity { get; set; }

        public String price { get; set; }
        
        public String date { get; set; }

        public ExportItem(String _barcode, String _quantity, String _price, String _date)
        {
            barcode = _barcode;
            quantity = _quantity;
            price = _price;
            date = _date;
        }
    }
}
