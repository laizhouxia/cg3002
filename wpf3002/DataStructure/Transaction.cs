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
            items = new List<transactionItem>();
            totalQuantity = 0;
            totalPrice = 0;
        }
        public List<transactionItem> items { get; set; }
        public DateTime date { get; set; }
        public Int32 totalQuantity { get; set; }
        public Double totalPrice { get; set; }
        public void add(String barcode,String quantity,String price)
        {
            try
            {
                items.Add(new transactionItem(barcode,quantity,price));
                totalQuantity++;
            }
            catch
            {
            }
        }
    }

}
