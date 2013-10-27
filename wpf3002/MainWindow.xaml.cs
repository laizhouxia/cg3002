﻿using System;
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
        String todayDate = "30/9/2013";
        String todayDateForFileName = "30_9_2013";
        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
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

        ObservableCollection<DataStructure.transactionItem> _oneTransaction = new ObservableCollection<DataStructure.transactionItem>();
        public ObservableCollection<DataStructure.transactionItem> oneTransaction
        {
            get
            {
                return this._oneTransaction;
            }
            set
            {
                if (value != this._oneTransaction)
                {
                    this._oneTransaction = value;
                }
            }
        }
        ObservableCollection<DataStructure.TransactionViewItem> _oneTransactionView = new ObservableCollection<DataStructure.TransactionViewItem>();
        public ObservableCollection<DataStructure.TransactionViewItem> oneTransactionView
        {
            get
            {
                return this._oneTransactionView;
            }
            set
            {
                if (value != this._oneTransactionView)
                {
                    this._oneTransactionView = value;
                }
            }
        }
        ObservableCollection<DataStructure.PriceTag> _pricetag = new ObservableCollection<DataStructure.PriceTag>();
        public ObservableCollection<DataStructure.PriceTag> pricetag
        {
            get
            {
                return this._pricetag;
            }
            set
            {
                if (value != this._pricetag)
                {
                    this._pricetag = value;
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
            MessageBox.Show("Sync complete!!");
            initialPriceTagId();
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
            return new DataStructure.Item(name.Text, barcode.Text, daily_price.Text, current_stock.Text, minimum_stock.Text, bundle_unit.Text, category.Text, manufacturer.Text);
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
                                //transaction.add(oneItemBarcode, receivedQuantity);
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
                        {
                            ListViewPriceList.SelectedItem = i;
                            ListViewPriceList.ScrollIntoView(ListViewPriceList.SelectedItem);
                            ListViewItem item = ListViewPriceList.ItemContainerGenerator.ContainerFromItem(ListViewPriceList.SelectedItem) as ListViewItem;
                            item.Focus();
                            selectedItem = (DataStructure.Item)item.Content;
                            setAllTextBox(selectedItem);
                        }

                }
                else
                {
                    MessageBox.Show("Cannot find an item with barcode:" + barcodeSearch.Text);
                }
        }

        private void BarcodeSearchTransaction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                if (testBarcode(barcodeSearchTransaction.Text))
                {
                    foreach (var i in _allItems)
                        if (i.barcode == barcodeSearchTransaction.Text)
                        {
                            ListViewLessInfo.SelectedItem = i;
                            ListViewLessInfo.ScrollIntoView(ListViewLessInfo.SelectedItem);
                            ListViewItem item = ListViewLessInfo.ItemContainerGenerator.ContainerFromItem(ListViewLessInfo.SelectedItem) as ListViewItem;
                            item.Focus();
                        };
                }
                else
                {
                    MessageBox.Show("Cannot find an item with barcode:" + barcodeSearchTransaction.Text);
                }
        }
        DataStructure.Item selectedItemForLessInfo;
        DataStructure.Item selectedItemForLessInfo2;
        private void LessInfoSelectionChanged(object sender, SelectionChangedEventArgs e)
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
                selectedItemForLessInfo = (DataStructure.Item)item.Content;
                quantity.Text = "";
            }
        }

        DataStructure.Transaction transactionFromPC = new DataStructure.Transaction();

        private void addTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItemForLessInfo != null && quantity.Text.ToString() != "")
            {

                int tempQuantityTotal;
                Boolean isIneter = Int32.TryParse(quantity.Text.ToString(), out tempQuantityTotal);
                if (!isIneter)
                    MessageBox.Show("Please type in correct number!!");
                else
                {
                    for (int j = 0; j < data.Count; j++)
                        if (data[j].barcode == selectedItemForLessInfo.barcode)
                            if (Convert.ToInt32(selectedItemForLessInfo.current_stock) >= Convert.ToInt32(quantity.Text.ToString()))
                            {
                                Boolean isChanged = false;
                                for (int i = 0; i < transactionFromPC.items.Count; i++)
                                    if (transactionFromPC.items[i].barcode == selectedItemForLessInfo.barcode)
                                    {
                                        tempQuantityTotal += Convert.ToInt32(transactionFromPC.items[i].quantity);
                                        transactionFromPC.items[i] = new DataStructure.transactionItem(transactionFromPC.items[i].barcode, tempQuantityTotal.ToString(), transactionFromPC.items[i].price);
                                        isChanged = true;
                                        break;
                                    }
                                if (!isChanged)
                                {
                                    for (int i = 0; i < data.Count; i++)
                                        if (data[i].barcode == selectedItemForLessInfo.barcode)
                                        {
                                            transactionFromPC.add(selectedItemForLessInfo.barcode, quantity.Text.ToString(), data[i].daily_price);
                                            break;
                                        }
                                    _allItems.Clear();
                                    foreach (var i in data)
                                        _allItems.Add(i);
                                    ListViewLessInfo.SelectedItem = selectedItemForLessInfo;
                                }
                                else
                                {
                                    _allItems.Clear();
                                    foreach (var i in data)
                                        _allItems.Add(i);
                                    ListViewLessInfo.SelectedItem = selectedItemForLessInfo;
                                }
                                for (int i = 0; i < data.Count; i++)
                                    if (data[i].barcode == selectedItemForLessInfo.barcode)
                                        data[i].current_stock = (Convert.ToInt32(selectedItemForLessInfo.current_stock) - Convert.ToInt32(quantity.Text.ToString())).ToString();
                                break;
                            }
                            else
                            {
                                MessageBox.Show("Please type in correct number..");
                                break;
                            }
                }
            }
            else
                MessageBox.Show("Please select both item and quantity!!");
            _oneTransaction.Clear();
            for (int i = 0; i < transactionFromPC.items.Count; i++)
            {
                _oneTransaction.Add(transactionFromPC.items[i]);
            }
        }
        DataStructure.transactionItem selectedItemForTransaction;
        private void TransactionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem item = new ListBoxItem();
            try
            {
                item = this.ListViewTransaction.ItemContainerGenerator.ContainerFromIndex(this.ListViewTransaction.SelectedIndex) as ListBoxItem;
            }
            catch
            {
                item = null;
            }
            if (item != null)
            {
                deleteTransaction.IsEnabled = true;
                selectedItemForTransaction = (DataStructure.transactionItem)item.Content;
            }
            else
                deleteTransaction.IsEnabled = false;
        }

        private void deleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < data.Count; i++)
                if (data[i].barcode == selectedItemForTransaction.barcode)
                    data[i].current_stock = (Convert.ToInt32(data[i].current_stock) + Convert.ToInt32(selectedItemForTransaction.quantity)).ToString();
            for (int i = 0; i < transactionFromPC.items.Count; i++)
                if (transactionFromPC.items[i].barcode == selectedItemForTransaction.barcode)
                    transactionFromPC.items.RemoveAt(i);
            _allItems.Clear();
            _oneTransaction.Clear();
            for (int i = 0; i < transactionFromPC.items.Count; i++)
            {
                _oneTransaction.Add(transactionFromPC.items[i]);
            }
            foreach (var i in data)
                _allItems.Add(i);
            ListViewLessInfo.SelectedItem = selectedItemForLessInfo;
        }

        private void cancelTransaction_Click(object sender, RoutedEventArgs e)
        {
            for (int j = 0; j < transactionFromPC.items.Count; j++)
                for (int i = 0; i < data.Count; i++)
                    if (data[i].barcode == transactionFromPC.items[j].barcode)
                        data[i].current_stock = (Convert.ToInt32(data[i].current_stock) + Convert.ToInt32(transactionFromPC.items[j].quantity)).ToString();
            transactionFromPC.items.Clear();
            _allItems.Clear();
            _oneTransaction.Clear();
            foreach (var i in data)
                _allItems.Add(i);
            ListViewLessInfo.SelectedItem = selectedItemForLessInfo;
        }

        private void saveTransaction_Click(object sender, RoutedEventArgs e)
        {
            transactionFromPC.date = todayDate;
            _wholeDayTransaction.Add(transactionFromPC);
            transactionFromPC = new DataStructure.Transaction();
            _oneTransaction.Clear();
            for (int i = 0; i < transactionFromPC.items.Count; i++)
            {
                _oneTransaction.Add(transactionFromPC.items[i]);
            }
        }

        private void AllTransactionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem item = new ListBoxItem();
            try
            {
                item = this.ListViewAllTransaction.ItemContainerGenerator.ContainerFromIndex(this.ListViewAllTransaction.SelectedIndex) as ListBoxItem;
            }
            catch
            {
                item = null;
            }
            if (item != null)
            {
                DataStructure.Transaction tempTransaction = (DataStructure.Transaction)item.Content;
                _oneTransactionView.Clear();
                for (int i = 0; i < tempTransaction.items.Count; i++)
                    for (int j = 0; j < data.Count; j++)
                        if (data[j].barcode == tempTransaction.items[i].barcode)
                            _oneTransactionView.Add(new DataStructure.TransactionViewItem(data[j].name, data[j].barcode, data[j].daily_price, tempTransaction.items[i].quantity));
            }
        }
        string[] a = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        string[] b = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        private void initialPriceTagId()
        {
            for (int i = 0; i < a.Length; i++)
                for (int j = 0; j < b.Length; j++)
                    pricetagIDListView.Items.Add(a[i] + b[j]);
        }

        private void LessInfo2SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem item = new ListBoxItem();
            try
            {
                item = this.ListViewLessInfo2.ItemContainerGenerator.ContainerFromIndex(this.ListViewLessInfo2.SelectedIndex) as ListBoxItem;
            }
            catch
            {
                item = null;
            }
            if (item != null)
            {
                selectedItemForLessInfo2 = (DataStructure.Item)item.Content;
            }
        }

        private void addPriceTag_Click(object sender, RoutedEventArgs e)
        {
            if (pricetagIDListView.SelectedItem != null && selectedItemForLessInfo2 != null)
            {
                for (int i = 0; i < pricetag.Count; i++)
                    if (pricetag[i].pricetagID == pricetagIDListView.SelectedItem.ToString())
                        pricetag.RemoveAt(i);
                DataStructure.PriceTag tempTag = new DataStructure.PriceTag(selectedItemForLessInfo2.name, selectedItemForLessInfo2.barcode, selectedItemForLessInfo2.daily_price, pricetagIDListView.SelectedItem.ToString());
                pricetag.Add(tempTag);
            }
            else
            {
                MessageBox.Show("Please select both id and barcode!!");
            }
        }


        private void loadTransaction_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                ThreadPool.QueueUserWorkItem((x) =>
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {



                        String filename = dlg.FileName;
                        DataStructure.Transaction tempTransaction = new DataStructure.Transaction();
                        String transactionID = "";
                        foreach (String line in File.ReadAllLines(filename))
                        {
                            String[] tokens = line.Split(':');
                            String _barcode = tokens[3];
                            String _quantity = tokens[4];
                            String _price = "";

                            String[] _dates = tokens[5].Split('/');
                            DateTime _date = new DateTime(Convert.ToInt32(_dates[2]), Convert.ToInt32(_dates[1]), Convert.ToInt32(_dates[0]));
                            foreach (var i in data)
                                if (i.barcode == _barcode)
                                {
                                    if (_dates[0] == "30" && _dates[1] == "9")
                                    {
                                        _price = i.daily_price;
                                        i.current_stock = (Convert.ToInt32(i.current_stock) - Convert.ToInt32(_quantity)).ToString();
                                    }
                                    else
                                    {
                                        _price = i.daily_price;
                                    }
                                }
                            if (transactionID == tokens[0])
                            {
                                tempTransaction.add(_barcode, _quantity, _price);
                                tempTransaction.date = tokens[5];
                            }
                            else
                            {
                                transactionID = tokens[0];
                                wholeDayTransaction.Add(tempTransaction);
                                tempTransaction = new DataStructure.Transaction();
                                tempTransaction.add(_barcode, _quantity, _price);
                                tempTransaction.date = tokens[5];
                            }
                        }
                        wholeDayTransaction.Add(tempTransaction);
                    }));
                });
            }
            _allItems.Clear();
            foreach (var i in data)
                _allItems.Add(i);
            MessageBox.Show("Loading Transaction Successfully!!");
        }

        private async void Sync_Click(object sender, RoutedEventArgs e)
        {
            data.Clear();
            _allItems.Clear();
            await initial();
            MessageBox.Show("Sync complete!!");
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            List<DataStructure.ExportItem> allExportItem = new List<DataStructure.ExportItem>();
            foreach (var i in _wholeDayTransaction)
            {
                if (i.date == todayDate)
                {

                    foreach (var k in i.items)
                    {
                        Boolean isBarcodeExist = false;
                        foreach (var j in allExportItem)
                            if (j.barcode == k.barcode)
                            {
                                isBarcodeExist = true;
                                j.quantity = (Convert.ToInt32(k.quantity) + Convert.ToInt32(j.quantity)).ToString();
                            }
                        if (!isBarcodeExist)
                            allExportItem.Add(new DataStructure.ExportItem(k.barcode, k.quantity, k.price, todayDate));
                    }
                }
            }
            String filename = "data" + todayDateForFileName + ".txt";

            Microsoft.Win32.SaveFileDialog savefile = new Microsoft.Win32.SaveFileDialog();
            savefile.DefaultExt = ".text"; // Default file extension
            savefile.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = savefile.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                StreamWriter sw = new StreamWriter(savefile.FileName);
                foreach (var i in allExportItem)
                    sw.WriteLine(i.barcode + ":" + i.quantity + ":" + i.price + ":" + i.date);
                sw.Close();
                MessageBox.Show("Export File Successful!!!");
            }

        }

    }
}
