using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading;



namespace wpf3002
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
            //initial();
            //ListViewPriceList.ItemsSource = priceList.allItems;
        }

        private async Task initial()
        {
            String response = await Functions.RequestSender.GetPriceListAsync();
            if (response != null)
            {
                ObservableCollection<DataStructure.Item> temp = (ObservableCollection<DataStructure.Item>)JsonConvert.DeserializeObject<ObservableCollection<DataStructure.Item>>(response);
                for (int i = 0; i < 500; i++)
                {
                    //textBoxUI.Text += temp[i].barcode + "\r\n";
                    _allItems.Add(temp[i]);
                }
                //textBoxUI.Text += "finish";
            }
            else
            {
                //textBoxUI.Text += "response is empty";
            }
        }


        ObservableCollection<DataStructure.Item> _allItems = new ObservableCollection<DataStructure.Item>();

        public ObservableCollection<DataStructure.Item> allItems
        {
            get
            {
                return this._allItems;
            }
            //set
            //{
            //    if (value != this._allItems)
            //    {
            //        this._allItems = value;
            //    }
            //}
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await initial();
            ThreadPool.QueueUserWorkItem((x) =>
            {
                //while (true)
                //{
                //    Dispatcher.BeginInvoke((Action)(() =>
                //    {
                //        // mFileNames.Add(new FileInfo("X"));

                //        _allItems.Add(new DataStructure.Item("1", "1", "1", "1", "1", "1", "1", "1"));
                //    }));
                //    Thread.Sleep(500);
                //}
            });
        }
    }
}
