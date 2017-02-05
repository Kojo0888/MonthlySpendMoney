using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MSMoney.Model;
using System.Windows.Input;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MSMoney
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public Entry Current { set; get; } = new Entry();

        public string Name { get; set; }

        public string Price { get; set; }

        public string Amount { get; set; }

        public List<Entry> Elements
        {
            get
            {
                if (root == null)
                    return null;
                return root.Entries;
            }
        } 

        private Root root;

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal ThisMonthPrice { set; get; }

        public decimal PreviousMonthPrice { set; get; }

        public decimal CurrentPrice { set; get; }

        private string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                   "MothlySpendMoney\\" + DateTime.Now.Year + "_" + DateTime.Now.Month + ".xml");

        public void Init()
        {
            string dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "MothlySpendMoney");

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            try
            {

                if (!File.Exists(path))
                {
                    XmlDocument doc = new XmlDocument();
                    doc = new XmlDocument();
                    doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
                    doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "Root", ""));
                    doc.Save(Path.Combine(dirPath, DateTime.Now.Year + "_" + DateTime.Now.Month + ".xml"));
                }

                root = DeserializeRoot(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error ocured:");
            }

            CountPrices();
            CountLastMonthPrices();
        }

        private void CountPrices()
        {
            decimal countedPrice = 0;
            foreach (var item in root.Entries)
            {
                countedPrice += item.Price * item.Amount;
            }
            ThisMonthPrice = countedPrice;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ThisMonthPrice"));
            //PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Elements"));
        }

        private void CountLastMonthPrices()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "MothlySpendMoney\\" + DateTime.Now.AddMonths(-1).Year 
                + "_" + DateTime.Now.AddMonths(-1).Month + ".xml");

            if (File.Exists(path))
            {
                Root lastMonthRoot = DeserializeRoot(path);
                    
                decimal countedPrice = 0;
                foreach (var item in lastMonthRoot.Entries)
                {
                    countedPrice += item.Price * item.Amount;
                }
                PreviousMonthPrice = countedPrice;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("PreviousMonthPrice"));
            }
        }

        private Root DeserializeRoot(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Root));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                return (Root)serializer.Deserialize(fs);
            }
        }

        public bool SaveNewEntry()
        {
            Price = Price.Replace('.',',');

            try
            {
                root.Entries.Add(new Entry() {
                    Amount = int.Parse(Amount),
                    Name = Name,
                    Price = decimal.Parse(Price)
                });
            }
            catch(FormatException)
            {
                return false;
            }

            CurrentPrice += decimal.Parse(Price) * int.Parse(Amount);
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs("CurrentPrice"));

            CountPrices();

            return true;
        }

        public void SaveData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Root));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                serializer.Serialize(fs,root);
            }
        }

        public void ShowMessageWindow()
        {
            MessageBox.Show("Nazwa:" + Current.Name);
        }
    }
}
