using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace wpf3002.DataStructure
{
    [DataContract]
    public class Item
    {
        [DataMember(Name = "name")]
        public String name { get; set; }

        [DataMember(Name = "barcode")]
        public String barcode { get; set; }

        [DataMember(Name = "daily_price")]
        public String daily_price { get; set; }

        [DataMember(Name = "current_stock")]
        public String current_stock { get; set; }

        [DataMember(Name = "minimum_stock")]
        public String minimum_stock { get; set; }

        [DataMember(Name = "bundle_unit")]
        public String bundle_unit { get; set; }

        [DataMember(Name = "category")]
        public String category { get; set; }

        [DataMember(Name = "manufacturer")]
        public String manufacturer { get; set; }

        public Item(String _name, String _barcode, String _daily_price, String _current_stock, String _minimum_stock, String _bundle_unit, String _category, String _manufacturer)
        {
            name = _name;
            barcode = _barcode;
            daily_price = _daily_price;
            current_stock = _current_stock;
            minimum_stock = _minimum_stock;
            bundle_unit = _bundle_unit;
            category = _category;
            manufacturer = _manufacturer;
        }
    }
}
