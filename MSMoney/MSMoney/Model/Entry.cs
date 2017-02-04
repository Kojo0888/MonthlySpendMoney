using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace MSMoney.Model
{
    [XmlSerializerFormat]
    [DataContract]
    public class Root
    {
        [DataMember]
        public List<Entry> Entries { set; get; } = new List<Entry>();
    }

    [DataContract]
    public class Entry
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public float Price { get; set; }

        [DataMember]
        public int Amount { get; set; }
    }
}
