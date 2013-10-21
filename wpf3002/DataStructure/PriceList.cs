//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.ComponentModel;
//using System.Collections.ObjectModel;
//using System.Net.NetworkInformation;
//using System.Net.Http;
//using Newtonsoft.Json;

//namespace wpf3002.DataStructure
//{

//    class PriceList : INotifyPropertyChanged
//    {
//        public PriceList(String jsonString)
//        {
//            //allItems = new ObservableCollection<Item>();
//            //allItems.CollectionChanged += _allItems_CollectionChanged;
//            _allItems = (ObservableCollection<Item>)JsonConvert.DeserializeObject<ObservableCollection<Item>>(jsonString);
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        private ObservableCollection<Item> _allItems;

//        public ObservableCollection<Item> allItems
//        {
//            get 
//            {
//                if (_allItems == null)
//                {
//                    _allItems.Add(new Item("1", "1", "1", "1", "1", "1", "1", "1"));
//                }
//                return this._allItems; 
//            }
//            set
//            {
//                if (value != this._allItems)
//                {
//                    this._allItems = value;
//                    OnPropertyChanged("allItem");
//                }
//            }
//        }

//        //void _allItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
//        //{
//        //    string test = null;
//        //    //throw new NotImplementedException();
//        //}

//        protected void OnPropertyChanged(string name)
//        {
//            PropertyChangedEventHandler handler = PropertyChanged;
//            if (handler != null)
//            {
//                handler(this, new PropertyChangedEventArgs(name));
//            }
//        }
//    }
//}
