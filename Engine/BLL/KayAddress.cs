using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Kay.Global;
using Kay.DAL;

namespace Kay.BLL
{
    public class KayAddress
    {
        // Properties
        public String Line1 = String.Empty;
        public String Line2 = String.Empty;
        public String Suburb = String.Empty;
        public String City = String.Empty;
        public String Country = String.Empty;
        public String Html
        {
            get
            {
                StringBuilder _html = new StringBuilder();
                if (!String.IsNullOrEmpty(Line1)) _html.AppendLine(String.Format("{0}<br/>", Line1));
                if (!String.IsNullOrEmpty(Line2)) _html.AppendLine(String.Format("{0}<br/>", Line2));
                if (!String.IsNullOrEmpty(Suburb)) _html.Append(String.Format("{0} ", Suburb));
                if (!String.IsNullOrEmpty(City)) _html.Append(String.Format("{0} ", City));
                if (!String.IsNullOrEmpty(Country)) _html.Append(String.Format("{0} ", Country));
                return _html.ToString();
            }
        }

        public String Longitude = "31.062469482421875";
        public String Latitude = "-17.829759683729208";

        // Constructor
        public KayAddress()
        {
        }
        public KayAddress(XmlNode Node)
        {
            SetProperties(Node);
        }

        // Properties
        private void SetProperties(XmlNode Node)
        {
            Line1 = Node.ChildNodes[0].HasChildNodes ? Node.ChildNodes[0].FirstChild.Value : String.Empty;
            Line2 = Node.ChildNodes[1].HasChildNodes ? Node.ChildNodes[1].FirstChild.Value : String.Empty;
            Suburb = Node.ChildNodes[2].HasChildNodes ? Node.ChildNodes[2].FirstChild.Value : String.Empty;
            City = Node.ChildNodes[3].HasChildNodes ? Node.ChildNodes[3].FirstChild.Value : String.Empty;
            Country = Node.ChildNodes[4].HasChildNodes ? Node.ChildNodes[4].FirstChild.Value : String.Empty;

            Longitude = Node.ChildNodes[5] != null && Node.ChildNodes[5].HasChildNodes ? Node.ChildNodes[5].FirstChild.Value : String.Empty;
            Latitude = Node.ChildNodes[6] != null && Node.ChildNodes[6].HasChildNodes ? Node.ChildNodes[6].FirstChild.Value : String.Empty;
        }

        #region XML

        public XmlNode ToXmlNode()
        {
            // Start document
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");

            String title = String.Empty;
            if (String.IsNullOrEmpty(title)) title = Line1;
            if (String.IsNullOrEmpty(title)) title = Line2;
            if (String.IsNullOrEmpty(title)) title = Suburb;
            if (String.IsNullOrEmpty(title)) title = City;
            if (String.IsNullOrEmpty(title)) title = Country;
            if (String.IsNullOrEmpty(title)) title = Longitude + " / " + Latitude;

            // Object node
            writer.WriteStartElement("option");
            writer.WriteAttributeString("id", Utilities.GetRandomPasswordUsingGUID(12));
            writer.WriteAttributeString("title", title);
            writer.WriteElementString("line1", Line1);
            writer.WriteElementString("line2", Line2);
            writer.WriteElementString("suburb", Suburb);
            writer.WriteElementString("city", City);
            writer.WriteElementString("country", Country);
            writer.WriteElementString("longitude", Longitude);
            writer.WriteElementString("latitude", Latitude);
            writer.WriteEndElement();

            // Clean up
            writer.Flush();
            writer.Close();

            // Create XML document
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.ToString());

            // Return single node
            return doc.DocumentElement;
        }

        public String ToXmlString()
        {
            return ToXmlNode().OuterXml;
        }

        public static KayAddress FromXmlString(String Xml)
        {
            XmlNode node = Utilities.StringToXmlNode(Xml);
            return new KayAddress(node);
        }

        #endregion
    }
}
