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
        String todayDate = "30/9/2013";
        String todayDateForFileName = "30_9_2013";
        String todayDateForUserUpload = "2013-9-30";
        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
        }

        private async Task initial()
        {
            String response = "";
            progressText.Text = "Downloading data...";
            data = new ObservableCollection<DataStructure.Item>();
            String storeID = StoreNum.Text;
            for (int j = 1; j < 11; j++)
            {
                progressText.Text = "Downloading page "+j;
                response = await Functions.RequestSender.GetPriceListAsync("http://"+HQURL.Text+"/api/" + storeID + "/price_list_paged.json?page=" + j);
                 
                if (response != null)
                {
                    ObservableCollection<DataStructure.Item> tempData = (ObservableCollection<DataStructure.Item>)JsonConvert.DeserializeObject<ObservableCollection<DataStructure.Item>>(response);
                    for (int i = 0; i < tempData.Count; i++)
                    {
                        //textBoxUI.Text += temp[i].barcode + "\r\n";
                        data.Add(tempData[i]);
                    }
                    for (int i = 0; i < tempData.Count; i++)
                    {
                        //textBoxUI.Text += temp[i].barcode + "\r\n";
                        _allItems.Add(tempData[i]);
                    }
                    //textBoxUI.Text += "finish";
                }
                else
                {
                    //textBoxUI.Text += "response is empty";
                }
            }

            response = await Functions.RequestSender.GetPriceListAsync("http://" + HQURL.Text + "/api/members.json");
            if (response != null)
            {
                _member = (ObservableCollection<DataStructure.Member>)JsonConvert.DeserializeObject<ObservableCollection<DataStructure.Member>>(response);
                foreach (var i in _member)
                    i.credits = 0.0;
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
        ObservableCollection<DataStructure.Member> _member = new ObservableCollection<DataStructure.Member>();
        public ObservableCollection<DataStructure.Member> member
        {
            get
            {
                return this._member;
            }
            set
            {
                if (value != this._member)
                {
                    this._member = value;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            initialPriceTagId();
            portInit();
        }

        List<String> workingDevices = new List<string>();

        private async void portSend()
        {

            try
            {
                while (true)
                {
                    foreach (var i in workingDevices)
                    {
                        port.Write("I" + i);
                    }
                    await Task.Delay(3000);
                }
            }
            catch
            {
                Dispatcher.Invoke((Action)delegate() { progressText.Text = "Send failed.."; });
            }
        }

        private async void SyncDevices_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in _pricetag)
            {
                String name = i.name;
                String price = i.price;
                if (name.Length < 17)
                {
                    int temp = name.Length;
                    for (int j = 0; j < 16 - temp; j++)
                        name += " ";
                    name += "*";
                }
                if (name.Length < 40)
                {
                    int temp = name.Length;
                    for (int j = 0; j < 40 - temp; j++)
                        name += " ";
                    temp = price.Length;
                    for (int j = 0; j < 40 - temp; j++)
                        price += " ";
                }
                else
                {
                    
                    if (name.Length > 72)
                        name = name.Substring(0,72);
                    int temp = price.Length + name.Length;
                    for (int j = 0; j < 80 - temp; j++)
                        price += " ";
                }
                await Task.Delay(30);
                port.Write(i.pricetagID + name + price);
                progressText.Text = "Trying to send " + i.pricetagID + " " + name;
            }
            for(int i=0;i<c.Length;i++)
                for (int j = 0; j < b.Length; j++)
                {
                    await Task.Delay(30);
                    String tempID = c[i] + b[j];
                    if (!workingDevices.Contains(tempID))
                    {
                        port.Write("I" + tempID);
                        progressText.Text = "Trying to connect " + tempID;
                    }
                }
            progressText.Text = "Sync devices successful.. " + workingDevices.Count + " devices are working..";
        }
        
        private void portInit()
        {
            try
            {
                port.Open();
                port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                Thread oThread = new Thread(new ThreadStart(portSend));
                oThread.Start();
                progressText.Text = "Connect port successful..";
            }
            catch
            {
                Dispatcher.Invoke((Action)delegate() { progressText.Text = "Unable to connect port.."; });
                Dispatcher.Invoke((Action)delegate() { SyncDevices.IsEnabled = false; });
            }
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

        DataStructure.Transaction transaction = new DataStructure.Transaction();
        SerialPort port = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.Two);

        String message = "";
        Dictionary<String,DataStructure.Transaction> realtimeTranslantion = new Dictionary<string,DataStructure.Transaction>();
        Dictionary<String, List<String>> realtimeString = new Dictionary<string, List<string>>();

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Show all the incoming data in the port's buffer
            bool isRead = true;
            while (isRead)
            {
                try
                {
                    String tempmessage = port.ReadExisting();
                    if (tempmessage == null || tempmessage.Length == 0)
                        break;
                    if (tempmessage[0] == 'O')
                        message = tempmessage;
                    else
                        message += tempmessage;
                    //if(message!="O1AN")
                        Dispatcher.Invoke((Action)delegate() { testTextBox.Text += message + "\r\n"; });
                }
                catch (TimeoutException) {}
            }
            if (message.Length > 3)
            {
                String id = "" + message[1] + message[2];
                if (!workingDevices.Contains(id))
                    workingDevices.Add(id);
                if (realtimeTranslantion.ContainsKey(id))
                {
                    if (message[3] == 'E')
                    {
                        Dispatcher.Invoke((Action)delegate() { _wholeDayTransaction.Add(realtimeTranslantion[id]); });
                        Dispatcher.Invoke((Action)delegate() { syncfile(); });
                        realtimeTranslantion[id] = new DataStructure.Transaction();
                        realtimeTranslantion[id].casherID = id;
                        realtimeTranslantion[id].date = todayDate;
                        realtimeTranslantion[id].id = System.Guid.NewGuid().ToString("N");
                    }
                }
                else
                {
                    realtimeTranslantion.Add(id, new DataStructure.Transaction());
                    realtimeTranslantion[id].casherID = id;
                    realtimeTranslantion[id].date = todayDate;
                    realtimeTranslantion[id].id = System.Guid.NewGuid().ToString("N");
                }
                if (!realtimeString.ContainsKey(id))
                {
                    realtimeString.Add(id, new List<string>());
                    realtimeString[id].Add("");
                    realtimeString[id].Add("");
                    realtimeString[id].Add("");
                }
                if (message[3] == 'B' && message.Length == 12)
                {
                    String realtimeBarcode = message.Substring(4, 8);
                    String realtimePrice = "";
                    bool isFind = false;
                    foreach (var i in _allItems)
                        if (i.barcode == realtimeBarcode)
                        {
                            realtimePrice = i.daily_price;
                            String sendPrice = "";
                            foreach (var c in realtimePrice)
                                if (c != '.')
                                    sendPrice += c;
                            while(sendPrice.Length<6)
                                sendPrice = "0"+sendPrice;
                            port.Write("O" + id + "P" + sendPrice);
                            isFind = true;
                        }
                    if (isFind)
                    {
                        (realtimeString[id])[0] = realtimeBarcode;
                        (realtimeString[id])[2] = "";
                        (realtimeString[id])[2] = realtimePrice;
                    }
                    else
                    {
                        port.Write("O" + id + "R");
                    }
                }
                else if (message[3] == 'Q' && message.Length == 10)
                {
                    (realtimeString[id])[1] = message.Substring(4, 6);
                    if ((realtimeString[id])[0] != "" && (realtimeString[id])[2] != "")
                        realtimeTranslantion[id].items.Add(new DataStructure.transactionItem((realtimeString[id])[0], (realtimeString[id])[1], (realtimeString[id])[2]));
                }
                else if (message[3] == 'C')
                {
                    if(realtimeTranslantion[id].items.Count>0)
                        realtimeTranslantion[id].items.RemoveAt(realtimeTranslantion[id].items.Count-1);
                }

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
                    progressText.Text = "Cannot find an item with barcode:" + barcodeSearch.Text;
                    //MessageBox.Show("Cannot find an item with barcode:" + barcodeSearch.Text);
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
                        }
                }
                else
                {
                    progressText.Text = "Cannot find an item with barcode:" + barcodeSearchTransaction.Text;
                    //MessageBox.Show("Cannot find an item with barcode:" + barcodeSearchTransaction.Text);
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
            syscTotalPrice();
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
            syscTotalPrice();
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
            syscTotalPrice();
            syncfile();
        }

        private void saveTransaction_Click(object sender, RoutedEventArgs e)
        {
            transactionFromPC.date = todayDate;
            transactionFromPC.id = System.Guid.NewGuid().ToString("N");
            _wholeDayTransaction.Add(transactionFromPC);
            transactionFromPC = new DataStructure.Transaction();
            _oneTransaction.Clear();
            for (int i = 0; i < transactionFromPC.items.Count; i++)
            {
                _oneTransaction.Add(transactionFromPC.items[i]);
            }
            syscTotalPrice();
            syncfile();
        }

        private void syncOneTransactionView()
        {
            double totalPrice = 0;
            foreach (var i in _oneTransactionView)
                totalPrice += Convert.ToDouble(i.price) * Convert.ToDouble(i.quantity);
            TotalPriceTextBox2.Text = totalPrice.ToString();
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
                        {
                            _oneTransactionView.Add(new DataStructure.TransactionViewItem(data[j].name, data[j].barcode, data[j].daily_price, tempTransaction.items[i].quantity));
                            break;
                        }
            }
            syncOneTransactionView();
        }
        string[] a = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        string[] b = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        string[] c = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
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
        
        private void syscTotalPrice()
        {
            double totalPrice = 0;
            foreach (var i in _oneTransaction)
                totalPrice += Convert.ToDouble(i.price) * Convert.ToDouble(i.quantity);
            totalPriceTextBox.Text = totalPrice.ToString();
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
                            if (transactionID == tokens[0] || transactionID == "")
                            {
                                tempTransaction.add(_barcode, _quantity, _price);
                                tempTransaction.date = tokens[5];
                                transactionID = tokens[0];
                            }
                            else
                            {
                                tempTransaction.id = transactionID;
                                wholeDayTransaction.Add(tempTransaction);
                                transactionID = tokens[0];
                                tempTransaction = new DataStructure.Transaction();
                                tempTransaction.add(_barcode, _quantity, _price);
                                tempTransaction.date = tokens[5];
                            }
                        }
                        tempTransaction.id = transactionID;
                        wholeDayTransaction.Add(tempTransaction);
                    }));
                });
            }
            _allItems.Clear();
            foreach (var i in data)
                _allItems.Add(i);
            progressText.Text = "Loading Transaction Successfully!!";
            //MessageBox.Show("Loading Transaction Successfully!!");
        }

        private async void Sync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                data.Clear();
            }
            catch
            {
            }
            _allItems.Clear();
            await initial();
            syncfile();
            progressText.Text = "Sync complete!!";
            
            //MessageBox.Show("Sync complete!!");
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void syncfile()
        {
            String filename = "transactions.txt";
            StreamWriter sw = new StreamWriter(filename);
            foreach (var i in _wholeDayTransaction)
                foreach (var j in i.items)
                    sw.WriteLine(i.id + ":" + i.casherID + ":" + j.barcode + ": " + ":" + j.quantity + ":" + i.date);
            sw.Close();

            filename = "items.txt";
            sw = new StreamWriter(filename);
            foreach (var i in _allItems)
                sw.WriteLine(i.barcode + ":" + i.name + ":" + i.daily_price + ":" + i.current_stock + ":" + i.minimum_stock + ":" + i.bundle_unit + ":" + i.category + ":" + i.manufacturer);
            sw.Close();

            filename = "member.txt";
            sw = new StreamWriter(filename);
            foreach (var i in _member)
                sw.WriteLine(i.phone + ":" + i.credits*5 + ":" + i.credits + ":" + todayDateForUserUpload);
            sw.Close();
        }

        private async void UploadFile_Click(object sender, RoutedEventArgs e)
        {

            new WebClient().UploadFile("http://" + HQURL.Text + "/api/" + StoreNum.Text + "/members.json", "POST", @"member.txt");

            String response = await Functions.RequestSender.GetPriceListAsync("http://" + HQURL.Text + "/api/members.json");
            if (response != null)
            {
                _member = (ObservableCollection<DataStructure.Member>)JsonConvert.DeserializeObject<ObservableCollection<DataStructure.Member>>(response);
                foreach (var i in _member)
                    i.credits = 0.0;
            }
            else
            {
                //textBoxUI.Text += "response is empty";
            }

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
            String filename = "data" + ".txt";

            StreamWriter sw = new StreamWriter(filename);
            foreach (var i in allExportItem)
                sw.WriteLine(i.barcode + ":" + i.quantity + ":" + i.price + ":" + i.date);
            sw.Close();

            new WebClient().UploadFile("http://"+HQURL.Text+"/api/" + StoreNum.Text + "/transaction.json", "POST", @"data.txt");

            _wholeDayTransaction = new ObservableCollection<DataStructure.Transaction>();
            progressText.Text = "Upload successful...";
        }

        private void LoadTransaction_Item_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((x) =>
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    String filename = "transactions.txt";
                    DataStructure.Transaction tempTransaction = new DataStructure.Transaction();
                    String transactionID = "";
                    foreach (String line in File.ReadAllLines(filename))
                    {
                        String[] tokens = line.Split(':');
                        String _barcode = tokens[2];
                        String _quantity = tokens[4];
                        String _price = "";

                        String[] _dates = tokens[5].Split('/');
                        DateTime _date = new DateTime(Convert.ToInt32(_dates[2]), Convert.ToInt32(_dates[1]), Convert.ToInt32(_dates[0]));
                        foreach (var i in data)
                            if (i.barcode == _barcode)
                            {
                                _price = i.daily_price;
                            }
                        if (transactionID == tokens[0] || transactionID == "")
                        {
                            tempTransaction.add(_barcode, _quantity, _price);
                            tempTransaction.date = tokens[5];
                            transactionID = tokens[0];
                        }
                        else
                        {
                            tempTransaction.id = transactionID;
                            wholeDayTransaction.Add(tempTransaction);
                            transactionID = tokens[0];
                            tempTransaction = new DataStructure.Transaction();
                            tempTransaction.add(_barcode, _quantity, _price);
                            tempTransaction.date = tokens[5];
                        }
                    }
                    tempTransaction.id = transactionID;
                    if(tempTransaction.items.Count!=0)
                        wholeDayTransaction.Add(tempTransaction);
                }));
            });
            string[] lines = System.IO.File.ReadAllLines(@"items.txt");
            data = new ObservableCollection<DataStructure.Item>();
            foreach (string line in lines)
            {
                string[] splitWords = line.Split(':');
                data.Add(new DataStructure.Item(splitWords[1],splitWords[0],splitWords[2],splitWords[3],splitWords[4],splitWords[5],splitWords[6],splitWords[7]));
            }

            _allItems.Clear();
            foreach (var i in data)
                _allItems.Add(i);
            progressText.Text = "Loading Previous Info Successfully!!";
        }

        private void SaveSetting_Click(object sender, RoutedEventArgs e)
        {
            portInit();
        }
    }
}
