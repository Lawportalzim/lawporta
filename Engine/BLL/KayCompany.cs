using System;
using System.Collections;
using System.Data;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Xml;
using Kay.Global;
using Kay.DAL;

namespace Kay.BLL
{
    /// <summary>
    /// Company
    /// </summary>
    public class KayCompany
    {
        // Properties
        public int Id = 0;
        public String Name = String.Empty;                
        public String EmailAddress = String.Empty;
        public String Telephone = String.Empty;
        public String ContactPerson = String.Empty;
        public KayAddress Address = new KayAddress();
        public int NumberOfAccounts = 0;

        // Constructor
        public KayCompany()
        {
        }
        public KayCompany(int Id)
        {
            if (Id > 0)
            {
                DataView data = KayCompanyDAL.GetDetails(Id).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayCompany(XmlNode Node)
        {
            SetProperties(Node);   
        }
        public KayCompany(DataRow Row)
        {
            SetProperties(Row);
        }

        // Properties
        public void SetProperties(DataRow Row)
        {
            Id = int.Parse(Row["Companies_Id"].ToString());
            Name = Row["Companies_Name"].ToString();            
            EmailAddress = Row["Companies_EmailAddress"].ToString();
            Telephone = Row["Companies_Telephone"].ToString();
            ContactPerson = Row["Companies_ContactPerson"].ToString();
            Address = KayAddress.FromXmlString(Row["Companies_Address"].ToString());
            NumberOfAccounts = int.Parse(Row["Companies_NumberOfAccounts"].ToString());      
        }
        public void SetProperties(XmlNode Node)
        {
            
        }

        // List
        public static DataView List()
        {
            return KayCompanyDAL.GetList().DefaultView;
        }

        // Save
        public Boolean Save()
        {
            Boolean success = false;
            if (Id == 0)
            {
                success = Add();
            }
            else
            {
                success = Update();
            }
            return success;
        }

        // Add
        private Boolean Add()
        {
            // Insert database record
            Id = KayCompanyDAL.Add(Name, EmailAddress, Telephone, ContactPerson, Address.ToXmlString(), NumberOfAccounts);

            // Encrypt password using salt
            if (Id > 0) Update();

            // Success
            return Id > 0;
        }

        // Update
        private Boolean Update()
        {
            // Update database record
            Boolean Success = KayCompanyDAL.Update(Id, Name, EmailAddress, Telephone, ContactPerson, Address.ToXmlString(), NumberOfAccounts);

            // Success
            return Success;
        }

        // Delete
        public Boolean Delete()
        {
            Boolean success = false;
            if (Id > 0)
            {
                success = KayCompanyDAL.Delete(Id);
            }
            return success;
        }

        // Unique
        public Boolean UniqueEmailAddress()
        {
            return KayCompanyDAL.UniqueEmailAddress(Id, EmailAddress);
        }

        #region XML

        public XmlNode ToXmlNode()
        {
            // Start document
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);

            // Object node
            writer.WriteStartElement("user");
            writer.WriteAttributeString("id", Utilities.GetRandomPasswordUsingGUID(12));            
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

        public static KayCompany FromXmlString(String Xml)
        {
            XmlNode node = Utilities.StringToXmlNode(Xml);
            return new KayCompany(node);
        }

        #endregion

        #region Authentication

        #endregion
    }

    /// <summary>
    /// Groups
    /// </summary
   
    /// <summary>
    /// Company collection
    /// </summary>
    public class KayCompanyCollection : CollectionBase
    {
        // Constructors
        public KayCompanyCollection()
        {
        }
        public KayCompanyCollection(DataView Data)
        {
            foreach (DataRowView RowView in Data)
            {
                this.Add(new KayCompany(RowView.Row));
            }
        }
        public KayCompanyCollection(XmlDocument Xml)
        {
            foreach (XmlNode Node in Xml.DocumentElement)
            {
                KayCompany Company = new KayCompany(Node);
                this.Add(Company);
            }
        }

        #region XML

        public XmlDocument ToXml()
        {
            // Create XML skeleton
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");
            writer.WriteStartElement("users");

            // Loop through items
            foreach (KayCompany Company in this)
            {
                String node = Company.ToXmlNode().OuterXml;
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

        public static KayCompanyCollection FromXmlString(String Xml)
        {
            XmlDocument doc = Utilities.StringToXmlDocument(Xml);
            return new KayCompanyCollection(doc);
        }

        #endregion

        // Built-in methods
        public KayCompany this[int index]
        {
            get { return (KayCompany)List[index]; }
            set { List[index] = value; }
        }
        public int Add(KayCompany value)
        {
            return List.Add(value);
        }
        public int IndexOf(KayCompany value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, KayCompany value)
        {
            List.Insert(index, value);
        }
        public void Remove(KayCompany value)
        {
            List.Remove(value);
        }
        public bool Contains(KayCompany value)
        {
            return List.Contains(value);
        }
        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayCompany"))
                throw new ArgumentException("value must be of type KayCompany", "value");
        }
        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayCompany"))
                throw new ArgumentException("value must be of type KayCompany.", "value");
        }
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("Kay.BLL.KayCompany"))
                throw new ArgumentException("newValue must be of type KayCompany.", "newValue");
        }
        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayCompany"))
                throw new ArgumentException("value must be of type KayCompany.");
        }
    }

    /// <summary>
    /// Import users
    /// </summary>   
}
