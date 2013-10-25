using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf3002.DataStructure
{
    public class Transaction
    {
        public Transaction() 
        {
            items = new List<KeyValuePair<string, string>>();
            totalQuantity = 0;
            totalPrice = 0;
        }
        public List<KeyValuePair<String,String>> items{ get; set; }
        public DateTime date { get; set; }
        public Int32 totalQuantity { get; set; }
        public Double totalPrice { get; set; }
        public void add(String barcode,String quantity)
        {
            try
            {
                items.Add(new KeyValuePair<String, String>(barcode, quantity));
                totalQuantity++;
            }
            catch
            {
            }
        }
    }

}
