using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using Kay.Global;
using Kay.DAL;

namespace Kay.BLL
{
    /// <summary>
    /// Contact list
    /// </summary>
    public class KayContactList
    {
        // Constants
        public const int SUBSCRIBERS = 1;
        public const int CUSTOMERS = 2;

        // Properties
        public int Id = 0;
        public String Title = String.Empty;
        public int UserCount = 0;
        private DataView _users;
        public DataView Users
        {
            get
            {
                if (_users == null)
                {
                    _users = KayContactListDAL.Users(Id).DefaultView;
                }
                return _users;
            }
        }

        // Constructor
        public KayContactList()
        {
        }
        public KayContactList(int Id)
        {
            if (Id > 0)
            {
                DataView data = KayContactListDAL.GetDetails(Id).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayContactList(XmlNode Node)
        {
            SetProperties(Node);
        }
        public KayContactList(DataRow Row)
        {
            SetProperties(Row);
        }

        // Properties
        private void SetProperties(DataRow Row)
        {
            Id = int.Parse(Row["Id"].ToString());
            Title = Row["Title"].ToString();
            UserCount = int.Parse(Row["UserCount"].ToString());
        }
        private void SetProperties(XmlNode Node)
        {
            Id = int.Parse(Node.ChildNodes[0].FirstChild.Value);
            Title = Node.ChildNodes[1].FirstChild.Value;
            UserCount = int.Parse(Node.ChildNodes[2].FirstChild.Value);
        }

        // List
        public static DataView List()
        {
            return KayContactListDAL.GetList().DefaultView;
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
            Id = KayContactListDAL.Add(Title);

            // Success
            return Id > 0;
        }

        // Update
        private Boolean Update()
        {
            // Update database record
            Boolean Success = KayContactListDAL.Update(Id, Title);

            // Success
            return Success;
        }

        // Update sort order
        public Boolean UpdateOrder(int ReferenceId, String Direction)
        {
            return KayContactListDAL.UpdateOrder(Id, ReferenceId, Direction);
        }

        // Delete
        public Boolean Delete()
        {
            if (Id > 0)
            {
                // Update database record
                Boolean Success = KayContactListDAL.Delete(Id);

                // Success
                return Success;
            }
            else
            {
                // Failed
                return false;
            }
        }

        // Title
        public Boolean UniqueTitle()
        {
            return KayContactListDAL.UniqueTitle(Id, Title);
        }

        // XML
        public XmlNode ToXmlNode()
        {
            // Start document
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");

            // Object node
            writer.WriteStartElement("contactlist");
            writer.WriteAttributeString("id", Utilities.GetRandomPasswordUsingGUID(12));
            writer.WriteAttributeString("title", Title);
            writer.WriteElementString("id", Id.ToString());
            writer.WriteElementString("title", Title);
            writer.WriteElementString("usercount", UserCount.ToString());
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

        #region Users

        // Add a user
        public Boolean AddUser(int UserId)
        {
            return AddUser(Id, UserId);
        }
        public static Boolean AddUser(int ListId, int UserId)
        {
            RemoveUser(ListId, UserId);
            return KayContactListDAL.AddUser(ListId, UserId);
        }

        // Remove a user
        public Boolean RemoveUser(int UserId)
        {
            return RemoveUser(Id, UserId);
        }
        public static Boolean RemoveUserFromAll(int UserId)
        {
            return KayContactListDAL.RemoveUser(UserId);
        }
        public static Boolean RemoveUser(int ListId, int UserId)
        {
            return KayContactListDAL.RemoveUser(ListId, UserId);
        }

        #endregion
    }

    /// <summary>
    /// Contact list collection
    /// </summary>
    public class KayContactListCollection : CollectionBase
    {
        // Constructors
        public KayContactListCollection()
        {
        }
        public KayContactListCollection(DataView Data)
        {
            foreach (DataRowView RowView in Data)
            {
                this.Add(new KayContactList(RowView.Row));
            }
        }
        public KayContactListCollection(XmlDocument Xml)
        {
            foreach (XmlNode Node in Xml.DocumentElement)
            {
                KayContactList Object = new KayContactList(Node);
                this.Add(Object);
            }
        }

        // XML
        public XmlDocument ToXml()
        {
            // Create XML skeleton
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");
            writer.WriteStartElement("contactlists");

            // Loop through items
            foreach (KayContactList DataObject in this)
            {
                String node = DataObject.ToXmlNode().OuterXml;
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

        // Built-in methods
        public KayContactList this[int index]
        {
            get { return (KayContactList)List[index]; }
            set { List[index] = value; }
        }
        public int Add(KayContactList value)
        {
            return List.Add(value);
        }
        public int IndexOf(KayContactList value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, KayContactList value)
        {
            List.Insert(index, value);
        }
        public void Remove(KayContactList value)
        {
            List.Remove(value);
        }
        public bool Contains(KayContactList value)
        {
            return List.Contains(value);
        }
        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayContactList"))
                throw new ArgumentException("value must be of type KayContactList", "value");
        }
        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayContactList"))
                throw new ArgumentException("value must be of type KayContactList.", "value");
        }
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("Kay.BLL.KayContactList"))
                throw new ArgumentException("newValue must be of type KayContactList.", "newValue");
        }
        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayContactList"))
                throw new ArgumentException("value must be of type KayContactList.");
        }
    }
}
