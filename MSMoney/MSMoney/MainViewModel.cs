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

        //private XmlDocument doc { set; get; } = new XmlDocument();

        private Root root;

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal ThisMonthPrice { set; get; }

        public decimal PreviousMonthPrice { set; get; }

        public void Init()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    DateTime.Now.Year + "_" + DateTime.Now.Month + ".xml");
            try
            {

                if (!File.Exists(path))
                {
                    XmlDocument doc = new XmlDocument();
                    doc = new XmlDocument();
                    doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
                    doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "Root", ""));
                    doc.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        DateTime.Now.Year + "_" + DateTime.Now.Month + ".xml"));
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Root));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    root = (Root)serializer.Deserialize(fs);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error ocured:");
            }

            CountPrices();
        }

        private void CountPrices()
        {
            decimal countedPrice = 0;
            foreach (var item in root.Entries)
            {
                countedPrice += (decimal)item.Price * item.Amount;
            }
            ThisMonthPrice = countedPrice;

            PropertyChanged.Invoke(this,new PropertyChangedEventArgs("ThisMonthPrice"));

            //ThisMonthPrice.NotifyPropertyChanged

            //this.PropertyChanged.
        }

        public void SaveNewEntry()
        {
            root.Entries.Add(new Entry() {
                Amount = Current.Amount,
                Name = Current.Name,
                Price = Current.Price
            });
            Current.Name = "";
            Current.Amount = 0;
            Current.Price = 0;

            CountPrices();

            //PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Name"));
            //PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Amount"));
            //PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Price"));
        }

        public void SaveData()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    DateTime.Now.Year + "_" + DateTime.Now.Month + ".xml");

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
