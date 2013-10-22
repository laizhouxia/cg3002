using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf3002.DataStructure
{
    class Transaction
    {
        public Transaction() { }
        public List<KeyValuePair<String,String>> oneItem{ get; set; }
        public DateTime name { get; set; }
        public void add(String barcode,String quantity)
        {
            oneItem.Add(new KeyValuePair<String,String>( barcode,quantity));
        }
    }
}
