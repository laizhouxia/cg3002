using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf3002.DataStructure
{
    public class PriceTag
    {
        public String name { get; set; }
        public String barcode { get; set; }
        public String price { get; set; }
        public String pricetagID { get; set; }
        public PriceTag(String _name, String _barcode, String _price, String _pricetagID)
        {
            name = _name;
            barcode = _barcode;
            price = _price;
            pricetagID = _pricetagID;
        }
    }
}
