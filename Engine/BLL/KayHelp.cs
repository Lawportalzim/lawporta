using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Xml;
using Kay.Global;
using Kay.DAL;

namespace Kay.BLL
{
    /// <summary>
    /// Articles
    /// </summary>
    public class KayHelp
    {
        // Properties
        public int Id = 0;
     
        public String Content = String.Empty;  

        // Constructor
        public KayHelp()
        {
        }
        public KayHelp(int Id)
        {
            if (Id > 0)
            {
                DataView data = KayHelpDAL.GetDetails(Id).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayHelp(XmlNode Node)
        {
            SetProperties(Node);
        }
        public KayHelp(DataRow Row)
        {
            SetProperties(Row);
        }

        // Properties
        private void SetProperties(DataRow Row)
        {
            Id = int.Parse(Row["Helps_Id"].ToString());                      
            Content = Row["Helps_Content"].ToString();            
        }
        private void SetProperties(XmlNode Node)
        {
            Id = int.Parse(Node.ChildNodes[0].FirstChild.Value);
                     
            Content = Node.ChildNodes[5].FirstChild.Value;  
        }

        // List
        public static DataView List()
        {
            return KayHelpDAL.GetList().DefaultView;
        }
        public static DataView LiveList()
        {
            return KayHelpDAL.GetLiveList().DefaultView;
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
            Id = KayHelpDAL.Add(Content);

            // Move files
            if (Id > 0) Update();

            // Success
            return Id > 0;
        }

        // Update
        private Boolean Update()
        {
            
            // Update database record
            Boolean Success = KayHelpDAL.Update(Id, Content);

            // Success
            return Success;
        }

        // Delete
        public Boolean Delete()
        {
            if (Id > 0)
            {
                // Update database record
                Boolean Success = KayHelpDAL.Delete(Id);

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
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");

            // Object node
            writer.WriteStartElement("article");
            writer.WriteAttributeString("id", Utilities.GetRandomPasswordUsingGUID(12));
            
            writer.WriteElementString("id", Id.ToString());
            
            writer.WriteStartElement("content");
            writer.WriteCData(Content);
            writer.WriteEndElement();
            
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
        public static KayHelp FromXmlString(String Xml)
        {
            XmlNode node = Utilities.StringToXmlNode(Xml);
            return new KayHelp(node);
        }

        #endregion
    }

    /// <summary>
    /// Article status
    /// </summary>
    public enum KayHelpStatus : int
    {
        Live = 1,
        Hidden = 2
    }

    /// <summary>
    /// Article collection
    /// </summary>
    public class KayHelpCollection : CollectionBase
    {
        // Constructors
        public KayHelpCollection()
        { 
        }
        public KayHelpCollection(DataView Data)
        {
            foreach (DataRowView RowView in Data)
            {
                this.Add(new KayHelp(RowView.Row));
            }
        }
        public KayHelpCollection(XmlDocument Xml)
        {
            foreach (XmlNode Node in Xml.DocumentElement)
            {
                KayHelp Object = new KayHelp(Node);
                this.Add(Object);
            }
        }

        #region XML

        public XmlDocument ToXml()
        {
            // Create XML skeleton
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");
            writer.WriteStartElement("articles");

            // Loop through items
            foreach (KayHelp Article in this)
            {
                String node = Article.ToXmlNode().OuterXml;
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
        public static KayHelpCollection FromXmlString(String Xml)
        {
            XmlDocument doc = Utilities.StringToXmlDocument(Xml);
            return new KayHelpCollection(doc);
        }

        #endregion

        // Built-in methods
        public KayHelp this[int index]
        {
            get { return (KayHelp)List[index]; }
            set { List[index] = value; }
        }
        public int Add(KayHelp value)
        {
            return List.Add(value);
        }
        public int IndexOf(KayHelp value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, KayHelp value)
        {
            List.Insert(index, value);
        }
        public void Remove(KayHelp value)
        {
            List.Remove(value);
        }
        public bool Contains(KayHelp value)
        {
            return List.Contains(value);
        }
        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayHelp"))
                throw new ArgumentException("value must be of type KayHelp", "value");
        }
        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayHelp"))
                throw new ArgumentException("value must be of type KayHelp.", "value");
        }
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("Kay.BLL.KayHelp"))
                throw new ArgumentException("newValue must be of type KayHelp.", "newValue");
        }
        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayHelp"))
                throw new ArgumentException("value must be of type KayHelp.");
        }
    }
}
