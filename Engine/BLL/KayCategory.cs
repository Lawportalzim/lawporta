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
    /// Category
    /// </summary>
    public class KayCategory
    {
        // Properties
        public int Id = 0;
        public int ParentId = 0;
        public KayCategoryType Type = KayCategoryType.Civil;
        public String Title = String.Empty;
        public Boolean Recycled = false;
        public String UrlPath
        {
            get
            {
                return Utilities.GenerateUrlPath(
                    Type, Id, Title);
            }
        }
        public KayCategoryStatus Status = KayCategoryStatus.Live;
        public Boolean Live
        {
            get
            {
                return Status == KayCategoryStatus.Live;
            }
            set
            {
                Status = value ? KayCategoryStatus.Live : KayCategoryStatus.Hidden;
            }
        }
        public String FolderPath
        {
            get
            {
                return String.Format("/Data/Category/{0}/", Id.ToString().PadLeft(8, '0'));
            }
        }
        private KayCaseCategoryDescriptionCollection _caseCategoryDescriptions;
        public KayCaseCategoryDescriptionCollection CaseCategoryDescriptions
        {
            get
            {
                if (_caseCategoryDescriptions == null)
                {
                    _caseCategoryDescriptions = new KayCaseCategoryDescriptionCollection(KayCaseCategoryDescription.ListByCategory(Id));
                }
                return _caseCategoryDescriptions;
            }
            set
            {
                _caseCategoryDescriptions = value;
            }
        }


        // Constructor
        public KayCategory()
        {
        }
        public KayCategory(int Id)
        {
            if (Id > 0)
            {
                DataView data = KayCategoryDAL.GetDetails(Id).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayCategory(string UrlPath, KayCategoryType Type)
        {
            if (!String.IsNullOrEmpty(UrlPath))
            {
                DataView data = KayCategoryDAL.GetDetails(UrlPath, (int)Type).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayCategory(XmlNode Node)
        {
            SetProperties(Node);
        }
        public KayCategory(DataRow Row)
        {
            SetProperties(Row);
        }

        // Properties
        private void SetProperties(DataRow Row)
        {
            Id = int.Parse(Row["Id"].ToString());
            ParentId = int.Parse(Row["ParentId"].ToString());
            Type = (KayCategoryType)Enum.ToObject(typeof(KayCategoryType), int.Parse(Row["Type"].ToString()));
            Title = Row["Title"].ToString();
            Status = (KayCategoryStatus)Enum.ToObject(typeof(KayCategoryStatus), int.Parse(Row["Status"].ToString()));
        }
        private void SetProperties(XmlNode Node)
        {
            Id = int.Parse(Node.ChildNodes[0].FirstChild.Value);
            Type = (KayCategoryType)Enum.ToObject(typeof(KayCategoryType), int.Parse(Node.ChildNodes[1].FirstChild.Value));
            Title = Node.ChildNodes[2].FirstChild.Value;
            Status = (KayCategoryStatus)Enum.ToObject(typeof(KayCategoryStatus), int.Parse(Node.ChildNodes[3].FirstChild.Value));
        }

        // List
        public static DataView List(KayCategoryType Type)
        {
            return KayCategoryDAL.GetList((int)Type).DefaultView;
        }
        public static DataView RecycledList()
        {
            return KayCategoryDAL.GetRecycledList().DefaultView;
        }
        public static DataView List(int ParentId)
        {
            return KayCategoryDAL.GetListByParent(ParentId).DefaultView;
        }
        public static DataView List(KayCategoryType Type, int ParentId)
        {
            return KayCategoryDAL.GetList((int)Type, ParentId).DefaultView;
        }
        public static DataView LiveList(KayCategoryType Type)
        {
            return KayCategoryDAL.GetLiveList((int)Type).DefaultView;
        }
        public static DataView LiveList(KayCategoryType Type, int ParentId)
        {
            return KayCategoryDAL.GetLiveListByParent((int)Type, ParentId).DefaultView;
        }
        public Boolean HasChildren()
        {
            return KayCategoryDAL.GetListByParent(Id).DefaultView.Count > 0;
        }
        //search
        public static DataView Find(String Keywords)
        {
            return KayCategoryDAL.Find(Keywords).DefaultView;
        }
        public static DataView Find(String Keywords, KayCategoryType Type)
        {
            return KayCategoryDAL.Find(Keywords, (int)Type).DefaultView;
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
            Id = KayCategoryDAL.Add((int)Type, Title, UrlPath, (int)Status, ParentId, Recycled);

            // Success
            return Id > 0;
        }

        // Update
        private Boolean Update()
        {
            // Update database record
            Boolean Success = KayCategoryDAL.Update(Id, (int)Type, Title, UrlPath, (int)Status, ParentId, Recycled);

            // Success
            return Success;
        }

        // Sort
        public Boolean UpdateOrder(int ReferenceId, String Direction)
        {
            return KayCategoryDAL.UpdateOrder(Id, ReferenceId, Direction);
        }

        // Delete
        public Boolean Delete()
        {
            if (Id > 0)
            {
                // Update database record
                Boolean Success = KayCategoryDAL.Delete(Id);

                // Remove folder and files
                if (Success) Utilities.DeleteFolder(FolderPath);

                // Success
                return Success;
            }
            else
            {
                // Failed
                return false;
            }
        }

        // Unique
        public Boolean Unique()
        {
            bool _unique = KayCategoryDAL.UniqueUrlPath(Id, (int)Type, UrlPath);
            return _unique;
        }
        public static Boolean Unique(KayCategoryType Type, String UrlPath)
        {
            return KayCategoryDAL.UniqueUrlPath(0, (int)Type, UrlPath);
        }

        #region XML

        // XML
        public XmlNode ToXmlNode()
        {
            // Start document
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");

            // Object node
            writer.WriteStartElement("category");
            writer.WriteAttributeString("id", Utilities.GetRandomPasswordUsingGUID(12));
            writer.WriteAttributeString("title", Title);
            writer.WriteElementString("id", Id.ToString());
            writer.WriteElementString("type", ((int)Type).ToString());
            writer.WriteElementString("title", Title);
            writer.WriteElementString("status", ((int)Status).ToString());
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
        public static KayCategory FromXmlString(String Xml)
        {
            XmlNode node = Utilities.StringToXmlNode(Xml);
            return new KayCategory(node);
        }

        #endregion
    }

    /// <summary>
    /// Category type
    /// </summary>
    public enum KayCategoryType : int
    {
        Civil = 1,
        Criminal = 2
    }

    /// <summary>
    /// Category status
    /// </summary>
    public enum KayCategoryStatus : int
    {
        Live = 1,
        Hidden = 2
    }

    /// <summary>
    /// Category collection
    /// </summary>
    public class KayCategoryCollection : CollectionBase
    {
        // Constructors
        public KayCategoryCollection()
        { 
        }
        public KayCategoryCollection(DataView Data)
        {
            foreach (DataRowView RowView in Data)
            {
                this.Add(new KayCategory(RowView.Row));
            }
        }
        public KayCategoryCollection(XmlDocument Xml)
        {
            foreach (XmlNode Node in Xml.DocumentElement)
            {
                KayCategory Object = new KayCategory(Node);
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
            writer.WriteStartElement("categories");

            // Loop through items
            foreach (KayCategory Category in this)
            {
                String node = Category.ToXmlNode().OuterXml;
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
        public static KayCategoryCollection FromXmlString(String Xml)
        {
            XmlDocument doc = Utilities.StringToXmlDocument(Xml);
            return new KayCategoryCollection(doc);
        }

        #endregion

        // Built-in methods
        public KayCategory this[int index]
        {
            get { return (KayCategory)List[index]; }
            set { List[index] = value; }
        }
        public int Add(KayCategory value)
        {
            return List.Add(value);
        }
        public int IndexOf(KayCategory value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, KayCategory value)
        {
            List.Insert(index, value);
        }
        public void Remove(KayCategory value)
        {
            List.Remove(value);
        }
        public bool Contains(KayCategory value)
        {
            return List.Contains(value);
        }
        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayCategory"))
                throw new ArgumentException("value must be of type KayCategory", "value");
        }
        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayCategory"))
                throw new ArgumentException("value must be of type KayCategory.", "value");
        }
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("Kay.BLL.KayCategory"))
                throw new ArgumentException("newValue must be of type KayCategory.", "newValue");
        }
        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayCategory"))
                throw new ArgumentException("value must be of type KayCategory.");
        }
    }
}
