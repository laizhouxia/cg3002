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
using System.IO.Ports;


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
            disableAllTextBox();
            //initial();
            //ListViewPriceList.ItemsSource = priceList.allItems;
        }

        private async Task initial()
        {
            String response = await Functions.RequestSender.GetPriceListAsync();
            if (response != null)
            {
                data = (ObservableCollection<DataStructure.Item>)JsonConvert.DeserializeObject<ObservableCollection<DataStructure.Item>>(response);
                for (int i = 0; i < data.Count; i++)
                {
                    //textBoxUI.Text += temp[i].barcode + "\r\n";
                    _allItems.Add(data[i]);
                }
                //textBoxUI.Text += "finish";
            }
            else
            {
                //textBoxUI.Text += "response is empty";
            }
        }

        ObservableCollection<DataStructure.Item> data;
        ObservableCollection<DataStructure.Item> _allItems = new ObservableCollection<DataStructure.Item>();
        DataStructure.Item selectedItem;
        DataStructure.Item prevSelectedItem;

        public ObservableCollection<DataStructure.Item> allItems
        {
            get
            {
                return this._allItems;
            }
            set
            {
                if (value != this._allItems)
                {
                    this._allItems = value;
                }
            }
        }

        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            _allItems.Clear();
            switch (sortBy)
            {
                case "name":
                    var sorted = from item in data
                                 orderby item.name
                                 select item;
                    foreach (var i in sorted)
                    {
                        _allItems.Add(i);
                    }
                    break;
                case "barcode":
                    sorted = from item in data
                             orderby item.barcode
                             select item;
                    foreach (var i in sorted)
                    {
                        _allItems.Add(i);
                    }
                    break;
                case "daily_price":
                    sorted = from item in data
                             orderby item.daily_price
                             select item;
                    foreach (var i in sorted)
                    {
                        _allItems.Add(i);
                    }
                    break;
                case "current_stock":
                    sorted = from item in data
                             orderby item.current_stock
                             select item;
                    foreach (var i in sorted)
                    {
                        _allItems.Add(i);
                    }
                    break;
                case "minimum_stock":
                    sorted = from item in data
                             orderby item.minimum_stock
                             select item;
                    foreach (var i in sorted)
                    {
                        _allItems.Add(i);
                    }
                    break;
                case "bundle_unit":
                    sorted = from item in data
                             orderby item.bundle_unit
                             select item;
                    foreach (var i in sorted)
                    {
                        _allItems.Add(i);
                    }
                    break;
                case "category":
                    sorted = from item in data
                             orderby item.category
                             select item;
                    foreach (var i in sorted)
                    {
                        _allItems.Add(i);
                    }
                    break;
                case "manufacturer":
                    sorted = from item in data
                             orderby item.manufacturer
                             select item;
                    foreach (var i in sorted)
                    {
                        _allItems.Add(i);
                    }
                    break;
                default:
                    break;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await initial();
            initialSerialPort();
            //ThreadPool.QueueUserWorkItem((x) =>
            //{
            //    while (true)
            //    {
            //        Dispatcher.BeginInvoke((Action)(() =>
            //        {
            //            // mFileNames.Add(new FileInfo("X"));

            //            _allItems.Add(new DataStructure.Item("1", "1", "1", "1", "1", "1", "1", "1"));
            //        }));
            //        Thread.Sleep(500);
            //    }
            //});
        }

        private void priceListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ListViewItem item = sender as ListViewItem;
            ListBoxItem item = new ListBoxItem();
            try
            {
                item = this.ListViewPriceList.ItemContainerGenerator.ContainerFromIndex(this.ListViewPriceList.SelectedIndex) as ListBoxItem;
            }
            catch
            {
                item = null;
            }
            if (item != null)
            {
                selectedItem = (DataStructure.Item)item.Content;
                setAllTextBox(selectedItem);
                enableAllTextBox();
            }
        }



        private void setAllTextBox(DataStructure.Item item)
        {
            name.Text = item.name;
            barcode.Text = item.barcode;
            daily_price.Text = item.daily_price;
            current_stock.Text = item.current_stock;
            minimum_stock.Text = item.minimum_stock;
            bundle_unit.Text = item.bundle_unit;
            category.Text = item.category;
            manufacturer.Text = item.manufacturer;
        }

        private void disableAllTextBox()
        {
            name.IsEnabled = false;
            barcode.IsEnabled = false;
            daily_price.IsEnabled = false;
            current_stock.IsEnabled = false;
            minimum_stock.IsEnabled = false;
            bundle_unit.IsEnabled = false;
            category.IsEnabled = false;
            manufacturer.IsEnabled = false;
        }
        private void enableAllTextBox()
        {
            name.IsEnabled = true;
            barcode.IsEnabled = true;
            daily_price.IsEnabled = true;
            current_stock.IsEnabled = true;
            minimum_stock.IsEnabled = true;
            bundle_unit.IsEnabled = true;
            category.IsEnabled = true;
            manufacturer.IsEnabled = true;
        }

        private DataStructure.Item readItem()
        {
            return new DataStructure.Item(name.Text,barcode.Text,daily_price.Text,current_stock.Text,minimum_stock.Text,bundle_unit.Text,category.Text,manufacturer.Text);
        }

        private Boolean testBarcode(string bar)
        {
            for (int i = 0; i < data.Count; i++)
                if (bar == data[i].barcode)
                    return true;
            return false;
        }

        private void modify_Click(object sender, RoutedEventArgs e)
        {
            String temp_barcode = selectedItem.barcode;
            prevSelectedItem = readItem();
            selectedItem = null;
            foreach (var itm in data)
            {
                if (itm.barcode == temp_barcode)
                {
                    data.Remove(itm);
                    break;
                }
            }
            foreach (var itm in _allItems)
            {
                if (itm.barcode == temp_barcode)
                {
                    _allItems.Remove(itm);
                    break;
                }
            }
            if (!testBarcode(barcode.Text))
            {
                data.Add(prevSelectedItem);
                _allItems.Add(prevSelectedItem);
            }
            else
                MessageBox.Show("Barcode is exist!!!");
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (!testBarcode(barcode.Text))
            {
                data.Add(readItem());
                _allItems.Add(readItem());
            }
            else
                MessageBox.Show("Barcode is exist!!!");
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            String temp_barcode = selectedItem.barcode;
            selectedItem = null;
            foreach (var itm in data)
            {
                if (itm.barcode == temp_barcode)
                {
                    data.Remove(itm);
                    break;
                }
            }
            foreach (var itm in _allItems)
            {
                if (itm.barcode == temp_barcode)
                {
                    _allItems.Remove(itm);
                    break;
                }
            }
        }

        static bool _continue;
        static SerialPort _serialPort;
        DataStructure.Transaction transaction = new DataStructure.Transaction();
        List<DataStructure.Transaction> wholeDayTransaction = new List<DataStructure.Transaction>();
        String oneItemBarcode = null;
        String oneItemPrice = null;

        public void initialSerialPort()
        {
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort("COM4");

            // Allow the user to set the appropriate properties.
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One;
            _serialPort.DataBits = 8;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            _continue = true;
            readThread.Start();

            while (_continue)
            {
                if (oneItemPrice == null && oneItemBarcode != null)
                {
                    
                    for (int i = 0; i < data.Count; i++)
                        if (oneItemBarcode == data[i].barcode)
                        {
                            oneItemPrice = data[i].daily_price;
                            _serialPort.WriteLine("O1"+oneItemPrice);
                        }
                }
            }

            readThread.Join();
            _serialPort.Close();
        }

        public void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    if (message != null && message.Length > 7)
                    {
                        if (message[0] == 'B' && message[1] == 'C')
                        {
                            string receivedBarcode = message.Substring(2, 6);
                            for (int i = 0; i < data.Count; i++)
                                if (receivedBarcode == data[i].barcode)
                                    oneItemBarcode = receivedBarcode;
                        }
                        if (message[0] == 'Q' && message[1] == 'T')
                        {
                            string receivedQuantity = message.Substring(2, 6);
                            if (oneItemBarcode != null)
                                transaction.add(oneItemBarcode, receivedQuantity);
                            oneItemBarcode = null;
                            oneItemPrice = null;
                        }
                        if (message[0] == 'T' && message[1] == 'H')
                        {
                            wholeDayTransaction.Add(transaction);
                            transaction = new DataStructure.Transaction();
                        }
                    }
                }
                catch (TimeoutException) { }
            }
        }
    }
}
