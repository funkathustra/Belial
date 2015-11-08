using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Xml;
using System.Collections.Generic;
namespace Belial.Models.Library
{
    [DebuggerStepThroughAttribute()]
    //[XmlTypeAttribute(AnonymousType = true)]
    //[XmlRootAttribute(Namespace = "", IsNullable = false)]
    [XmlRoot("MPL"), XmlType("MPL")]
    public partial class Response
    {


        public Response()
        {
            Items = new List<ResponseItem>();
        }

        [XmlElementAttribute("MPL")]
        public Mpl Mpl { get; set; }


        [XmlElementAttribute("Item")]
        public List<ResponseItem> Items { get; set; }


        [XmlAttributeAttribute()]
        public string Status { get; set; }
    }

    [DebuggerStepThroughAttribute()]
    [XmlTypeAttribute(AnonymousType = true)]
    public class Mpl
    {
        [XmlAttributeAttribute()]
        public string Version { get; set; }

        [XmlAttributeAttribute()]
        public string Title { get; set; }

        [XmlAttributeAttribute()]
        public string PathSeparator { get; set; }
    }

    [DebuggerStepThroughAttribute()]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class ResponseItem
    {
        public ResponseItem()
        {
            Fields = new List<ItemField>();
        }
        [XmlElementAttribute("Field")]
        public List<ItemField> Fields { get; set; }
    }

    [DebuggerStepThroughAttribute()]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class ItemField
    {
        [XmlAttributeAttribute()]
        public string Name { get; set; }

        [XmlTextAttribute()]
        public string Value { get; set; }
    }
}
