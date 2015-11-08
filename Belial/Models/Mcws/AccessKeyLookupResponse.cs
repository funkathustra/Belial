using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Belial.Models.Mcws
{
    public class AccessKeyLookupResponse
    {
        [XmlElement("keyid")]
        public string KeyId { get; set; }

        [XmlElement("ip")]
        public string Ip { get; set; }

        [XmlElement("port")]
        public string Port { get; set; }

        [XmlElement("localiplist")]
        public string LocalIpList { get; set; }

        [XmlElement("macaddresslist")]
        public string MacAddressList { get; set;  }
    }
}
