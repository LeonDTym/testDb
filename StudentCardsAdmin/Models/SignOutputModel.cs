using System;

namespace StudentCardsAdmin.Models
{
    [SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class Output
    {
        public string Log { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Ver { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SID { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Offset { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Create { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Result { get; set; }
        public ResultOperation Data { get; set; }
        [System.Xml.Serialization.XmlArrayItemAttribute("Device", IsNullable = false)]
        public OutputDevice[] Devices { get; set; }
    }

    public class ResultOperation
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Encode { get; set; }
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value { get; set; }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class OutputDevice
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Active { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SN { get; set; }
    }
}
