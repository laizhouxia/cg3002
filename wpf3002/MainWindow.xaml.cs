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
using System.Data;

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
            //disableAllTextBox();
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

        ObservableCollection<DataStructure.Transaction> _wholeDayTransaction = new ObservableCollection<DataStructure.Transaction>();
        public ObservableCollection<DataStructure.Transaction> wholeDayTransaction
        {
            get
            {
                return this._wholeDayTransaction;
            }
            set
            {
                if (value != this._wholeDayTransaction)
                {
                    this._wholeDayTransaction = value;
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

            //ThreadPool.QueueUserWorkItem((x) =>
            //{
            //    //while (true)
            //    //{
            //    //    Dispatcher.BeginInvoke((Action)(() =>
            //    //    {

            //    //    }));
            //    //    Thread.Sleep(500);
            //    //}
            //    initialSerialPort();
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
                //enableAllTextBox();
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


        static bool _continue;
        static SerialPort _serialPort;
        DataStructure.Transaction transaction = new DataStructure.Transaction();

        String oneItemBarcode = null;
        String oneItemPrice = null;

        public void initialSerialPort()
        {
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort("COM3");

            // Allow the user to set the appropriate properties.
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.Two;
            _serialPort.DataBits = 8;
            _serialPort.Encoding = Encoding.ASCII;
            _serialPort.ReadBufferSize = 128;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            //testTextBox.Text += "open serial port successful! \r\n";
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
                            _serialPort.WriteLine("O1" + oneItemPrice);
                        }
                }
                else if (oneItemBarcode == null)
                    _serialPort.WriteLine("I1B");
                else
                    _serialPort.WriteLine("I1Q");

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
                    testTextBox.Text += message + "\r\n";
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

        private void BarcodeSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                if (testBarcode(barcodeSearch.Text))
                {
                    foreach (var i in _allItems)
                        if (i.barcode == barcodeSearch.Text)
                            ListViewPriceList.SelectedItem = i;
                }
                else
                {
                    MessageBox.Show("Cannot find an item with barcode:"+barcodeSearch.Text);
                }
        }

        private void BarcodeSearchTransaction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                if (testBarcode(barcodeSearchTransaction.Text))
                {
                    foreach (var i in _allItems)
                        if (i.barcode == barcodeSearchTransaction.Text)
                            ListViewLessInfo.SelectedItem = i;
                }
                else
                {
                    MessageBox.Show("Cannot find an item with barcode:" + barcodeSearchTransaction.Text);
                }
        }
        DataStructure.Item selectedItemForTransaction;
        private void TransactionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem item = new ListBoxItem();
            try
            {
                item = this.ListViewLessInfo.ItemContainerGenerator.ContainerFromIndex(this.ListViewLessInfo.SelectedIndex) as ListBoxItem;
            }
            catch
            {
                item = null;
            }
            if (item != null)
            {
                selectedItemForTransaction = (DataStructure.Item)item.Content;
                quantity.Items.Clear();
                for (int i = 0; i < Convert.ToInt32(selectedItemForTransaction.current_stock); i++)
                {
                    quantity.Items.Add(i+1);
                }
            }
        }

        DataStructure.Transaction transactionFromPC = new DataStructure.Transaction();

        private void addTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItemForTransaction != null && quantity.SelectedItem != null)
                transactionFromPC.add(selectedItemForTransaction.barcode,quantity.SelectedItem.ToString());
            else
                MessageBox.Show("Please select both item and quantity!");
            ListViewTransaction.Items.Clear();
            for (int i = 0; i < transactionFromPC.items.Count; i++)
            {
                ListViewTransaction.Items.Add(new { Barcode = transactionFromPC.items.ElementAt(i).Key, Quantity = transactionFromPC.items.ElementAt(i).Value });
            }
        }

    }
}
