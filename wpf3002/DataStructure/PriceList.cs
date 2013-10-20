using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Net.Http;
using Newtonsoft.Json;

namespace wpf3002.DataStructure
{

    public class PriceList : INotifyPropertyChanged
    {
        public PriceList(String jsonString)
        {
            _allItem = (ObservableCollection<Item>)JsonConvert.DeserializeObject<ObservableCollection<Item>>(jsonString);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Item> _allItem;

        public ObservableCollection<Item> allItem
        {
            get { return this._allItem; }
            set
            {
                if (value != this._allItem)
                {
                    this._allItem = value;
                    OnPropertyChanged("allItem");
                }
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
