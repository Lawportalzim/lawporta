using System;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Xml;
using Kay.Global;
using Kay.DAL;

namespace Kay.BLL
{
    /// <summary>
    /// Page template
    /// </summary>
    public class KayPageTemplate
    {
        // Properties
        public int Id = 0;
        public String Title = String.Empty;
        public String UrlPath = String.Empty;
        public KayPagePlaceholderCollection Placeholders = new KayPagePlaceholderCollection();
        public String Filepath = String.Empty;
        public Boolean Greedy = false;
        public KayDevice Device = KayDevice.Public;
        public KayPageTemplateStatus Status = KayPageTemplateStatus.Live;
        public Boolean Live
        {
            get
            {
                return Status == KayPageTemplateStatus.Live;
            }
            set
            {
                Status = value ? KayPageTemplateStatus.Live : KayPageTemplateStatus.Hidden;
            }
        }

        // Constructor
        public KayPageTemplate()
        {
        }
        public KayPageTemplate(int Id)
        {
            if (Id > 0)
            {
                DataView data = KayPageTemplateDAL.GetDetails(Id).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayPageTemplate(XmlNode Node)
        {
            Id = int.Parse(Node.ChildNodes[0].FirstChild.Value);
            Title = Node.ChildNodes[1].FirstChild.Value;
            UrlPath = Node.ChildNodes[2].HasChildNodes ? Node.ChildNodes[2].FirstChild.Value: String.Empty;
            Placeholders = new KayPagePlaceholderCollection(Node.ChildNodes[3].FirstChild.Value);
            Filepath = Node.ChildNodes[4].FirstChild.Value;
            Status = (KayPageTemplateStatus)Enum.ToObject(typeof(KayPageTemplateStatus), int.Parse(Node.ChildNodes[5].FirstChild.Value));
            Device = (KayDevice)Enum.ToObject(typeof(KayDevice), int.Parse(Node.ChildNodes[6].FirstChild.Value));
        }
        public KayPageTemplate(DataRow Row)
        {
            SetProperties(Row);
        }

        // Properties
        private void SetProperties(DataRow Row)
        {
            Id = int.Parse(Row["Id"].ToString());
            Title = Row["Title"].ToString();
            UrlPath = Row["UrlPath"].ToString();
            Placeholders = new KayPagePlaceholderCollection(Row["Placeholders"].ToString());
            Filepath = Row["Filepath"].ToString();
            Greedy = (Row["Greedy"].ToString() == "1");
            Status = (KayPageTemplateStatus)Enum.ToObject(typeof(KayPageTemplateStatus), int.Parse(Row["Status"].ToString()));
            Device = (KayDevice)Enum.ToObject(typeof(KayDevice), int.Parse(Row["Device"].ToString()));            
        }

        // List
        public static DataView List()
        {
            return KayPageTemplateDAL.GetList().DefaultView;
        }
        public static DataView Available(KayDevice Platform)
        {
            DataView data = KayPageTemplateDAL.GetList().DefaultView;
            data.RowFilter = String.Format("Status = {0} And Device = {1}", (int)KayPageTemplateStatus.Live, (int)Platform);
            return data;
        }

        // Save
        public Boolean Save()
        {
            if (Id == 0) return Add();
            else return Update();
        }

        // Add
        private Boolean Add()
        {
            // Insert database record
            Id = KayPageTemplateDAL.Add(Title, UrlPath, Placeholders.ToXml().OuterXml, Filepath, Greedy ? 1 : 0, (int)Status, (int)Device);

            // Success
            return Id > 0;
        }

        // Update
        private Boolean Update()
        {
            // Update database record
            Boolean Success = KayPageTemplateDAL.Update(Id, Title, UrlPath, Placeholders.ToXml().OuterXml, Filepath, Greedy ? 1 : 0, (int)Status, (int)Device);

            // Success
            return Success;
        }

        // Update sort order
        public Boolean UpdateOrder(int ReferenceId, String Direction)
        {
            return KayPageTemplateDAL.UpdateOrder(Id, ReferenceId, Direction);
        }

        // Delete
        public Boolean Delete()
        {
            if (Id > 0)
            {
                // Update database record
                Boolean Success = KayPageTemplateDAL.Delete(Id);

                // Success
                return Success;
            }
            else
            {
                // Failed
                return false;
            }
        }

        #region XML

        public XmlNode ToXmlNode()
        {
            // Start document
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);

            // Object node
            writer.WriteStartElement("pagetemplate");
            writer.WriteElementString("id", Id.ToString());
            writer.WriteElementString("title", Title);
            writer.WriteElementString("urlpath", UrlPath);
            writer.WriteElementString("placeholders", Placeholders.ToXml().OuterXml);
            writer.WriteElementString("filepath", Filepath);
            writer.WriteElementString("status", ((int)Status).ToString());
            writer.WriteElementString("device", ((int)Device).ToString());
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

        public static KayPageTemplate FromXmlString(String Xml)
        {
            XmlNode node = Utilities.StringToXmlNode(Xml);
            return new KayPageTemplate(node);
        }

        #endregion
    }

    /// <summary>
    /// Page template status
    /// </summary>
    public enum KayPageTemplateStatus : int
    {
        Live = 1,
        Hidden = 2
    }

    /// <summary>
    /// Page template collection
    /// </summary>
    public class KayPageTemplateCollection : CollectionBase
    {
        // Constructors
        public KayPageTemplateCollection()
        {
        }
        public KayPageTemplateCollection(DataView Data)
        {
            foreach (DataRowView RowView in Data)
            {
                this.Add(new KayPageTemplate(RowView.Row));
            }
        }
        public KayPageTemplateCollection(String XmlString)
        {
            // Build collection from XML document
            XmlDocument Xml = new XmlDocument();
            Xml.LoadXml(XmlString);
            foreach (XmlNode Node in Xml.DocumentElement)
            {
                KayPageTemplate Object = new KayPageTemplate(Node);
                this.Add(Object);
            }
        }
        public KayPageTemplateCollection(XmlDocument Xml)
        {
            foreach (XmlNode Node in Xml.DocumentElement)
            {
                KayPageTemplate Object = new KayPageTemplate(Node);
                this.Add(Object);
            }
        }

        #region XML

        public XmlDocument ToXml()
        {
            // Create XML skeleton
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
            writer.WriteStartElement("items");

            // Loop through items
            foreach (KayPageTemplate Object in this)
            {
                String node = Object.ToXmlNode().OuterXml;
                writer.WriteRaw(node);
            }

            // Finish
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();

            // Create an XML document
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.ToString());

            // Return XML string
            return doc;
        }
        
        public String ToXmlString()
        {
            return ToXml().DocumentElement.OuterXml;
        }

        public static KayPageTemplateCollection FromXmlString(String Xml)
        {
            XmlDocument doc = Utilities.StringToXmlDocument(Xml);
            return new KayPageTemplateCollection(doc);
        }

        #endregion

        // Built-in methods
        public KayPageTemplate this[int index]
        {
            get { return (KayPageTemplate)List[index]; }
            set { List[index] = value; }
        }
        public int Add(KayPageTemplate value)
        {
            return List.Add(value);
        }
        public int IndexOf(KayPageTemplate value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, KayPageTemplate value)
        {
            List.Insert(index, value);
        }
        public void Remove(KayPageTemplate value)
        {
            List.Remove(value);
        }
        public bool Contains(KayPageTemplate value)
        {
            return List.Contains(value);
        }
        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayPageTemplate"))
                throw new ArgumentException("value must be of type KayPageTemplate", "value");
        }
        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayPageTemplate"))
                throw new ArgumentException("value must be of type KayPageTemplate.", "value");
        }
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("Kay.BLL.KayPageTemplate"))
                throw new ArgumentException("newValue must be of type KayPageTemplate.", "newValue");
        }
        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayPageTemplate"))
                throw new ArgumentException("value must be of type KayPageTemplate.");
        }
    }

    /// <summary>
    /// Page placeholder
    /// </summary>
    public class KayPagePlaceholder
    {
        // Properties
        public String Title = String.Empty;
        public String Options = String.Empty;
        public KayPagePlaceholderType Type = KayPagePlaceholderType.HTML;
        public String Name
        {
            get
            {
                return Regex.Replace(Title, @"[^a-zA-Z0-9]+", "_") + "_Placeholder";
            }
        }

        // Constructor
        public KayPagePlaceholder()
        {
        }
        public KayPagePlaceholder(String title)
        {
            Title = title;
        }
        public KayPagePlaceholder(XmlNode Node)
        {
            Title = Node.ChildNodes[0].HasChildNodes ? Node.ChildNodes[0].FirstChild.Value : String.Empty;
            Options = Node.ChildNodes[1].HasChildNodes ? Node.ChildNodes[1].FirstChild.Value : String.Empty;
            Type = Node.ChildNodes[2].HasChildNodes ? (KayPagePlaceholderType)Enum.Parse(typeof(KayPagePlaceholderType), Node.ChildNodes[2].FirstChild.Value) : KayPagePlaceholderType.Text;
        }

        #region XML

        public XmlNode ToXmlNode()
        {
            // Start document
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");

            // Root
            writer.WriteStartElement("placeholders");

            // Object node
            writer.WriteStartElement("placeholder");
            // ##
            writer.WriteAttributeString("id", Utilities.GetRandomPasswordUsingGUID(12));
            writer.WriteAttributeString("title", Title);
            // ##
            writer.WriteElementString("title", Title);
            writer.WriteStartElement("options");
            writer.WriteCData(Options);
            writer.WriteEndElement();
            writer.WriteElementString("type", Type.ToString());
            writer.WriteEndElement();

            // Close root
            writer.WriteEndElement();

            // Clean up
            writer.Flush();
            writer.Close();

            // Create XML document
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.ToString());

            // Return single node
            return doc.DocumentElement.FirstChild;
        }

        public String ToXmlString()
        {
            return ToXmlNode().OuterXml;
        }

        public static KayPagePlaceholder FromXmlString(String Xml)
        {
            XmlNode node = Utilities.StringToXmlNode(Xml);
            return new KayPagePlaceholder(node);
        }

        #endregion
    }

    /// <summary>
    /// Page placeholder type status
    /// </summary>
    public enum KayPagePlaceholderType : int
    {
        HTML = 1,
        Promo = 2,
        Image = 3,
        [DescriptionAttribute("Document collection")]
        DocumentCollection = 4,
        [DescriptionAttribute("Content collection")]
        ContentCollection = 5,
        Text = 6,
        Gallery = 7
    }

    /// <summary>
    /// User link collection
    /// </summary>
    public class KayPagePlaceholderCollection : CollectionBase
    {
        // Constructors
        public KayPagePlaceholderCollection()
        {
        }
        public KayPagePlaceholderCollection(String XmlString)
        {
            if (!String.IsNullOrEmpty(XmlString))
            {
                // Build collection from XML document
                XmlDocument Xml = new XmlDocument();
                Xml.LoadXml(XmlString);
                foreach (XmlNode Node in Xml.DocumentElement)
                {
                    KayPagePlaceholder Object = new KayPagePlaceholder(Node);
                    this.Add(Object);
                }
            }
        }
        public KayPagePlaceholderCollection(XmlDocument Xml)
        {
            foreach (XmlNode Node in Xml.DocumentElement)
            {
                KayPagePlaceholder Object = new KayPagePlaceholder(Node);
                this.Add(Object);
            }
        }

        // CSV
        public static KayPagePlaceholderCollection FromCsvString(String CsvString)
        {
            KayPagePlaceholderCollection _collection = new KayPagePlaceholderCollection();
            String[] _values = CsvString.Split(',');
            foreach (String _value in _values)
            {
                if (!String.IsNullOrEmpty(_value.Trim()))
                {
                    _collection.Add(new KayPagePlaceholder(_value.Trim()));
                }
            }
            return _collection;
        }
        public String ToCsvString()
        {
            StringBuilder _csv = new StringBuilder();
            foreach (KayPagePlaceholder placeholder in List)
            {
                _csv.Append(placeholder.Title + ", ");
            }
            return Regex.Replace(_csv.ToString(), @", $", "");
        }

        #region XML

        public XmlDocument ToXml()
        {
            // Create XML skeleton
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");
            writer.WriteStartElement("placeholders");

            // Loop through items
            foreach (KayPagePlaceholder Object in this)
            {
                String node = Object.ToXmlNode().OuterXml;
                writer.WriteRaw(node);
            }

            // Finish
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();

            // Create an XML document
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.ToString());

            // Return XML string
            return doc;
        }
        
        public String ToXmlString()
        {
            return ToXml().DocumentElement.OuterXml;
        }

        public static KayPagePlaceholderCollection FromXmlString(String Xml)
        {
            XmlDocument doc = Utilities.StringToXmlDocument(Xml);
            return new KayPagePlaceholderCollection(doc);
        }

        #endregion

        // Built-in methods
        public KayPagePlaceholder this[int index]
        {
            get { return (KayPagePlaceholder)List[index]; }
            set { List[index] = value; }
        }
        public int Add(KayPagePlaceholder value)
        {
            return List.Add(value);
        }
        public int IndexOf(KayPagePlaceholder value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, KayPagePlaceholder value)
        {
            List.Insert(index, value);
        }
        public void Remove(KayPagePlaceholder value)
        {
            List.Remove(value);
        }
        public bool Contains(KayPagePlaceholder value)
        {
            return List.Contains(value);
        }
        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayPagePlaceholder"))
                throw new ArgumentException("value must be of type KayPagePlaceholder", "value");
        }
        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayPagePlaceholder"))
                throw new ArgumentException("value must be of type KayPagePlaceholder.", "value");
        }
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("Kay.BLL.KayPagePlaceholder"))
                throw new ArgumentException("newValue must be of type KayPagePlaceholder.", "newValue");
        }
        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayPagePlaceholder"))
                throw new ArgumentException("value must be of type KayPagePlaceholder.");
        }
    }
}
