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
    /// Cases
    /// </summary>
    public class KayCaseCategoryDescription
    {
        // Properties
        public int Id = 0;
        public KayCategory Category = new KayCategory();
        public KayCase Case = new KayCase();
        public string Description = string.Empty;
        
        public String UrlPath
        {
            get
            {
                return Utilities.GenerateUrlPath(
                    "Cases", Id, Category.Id, Case.Id);
            }
        }

        // Constructor
        public KayCaseCategoryDescription()
        {
        }
        public KayCaseCategoryDescription(int Id)
        {
            if (Id > 0)
            {
                DataView data = KayCaseCategoryDescriptionDAL.GetDetails(Id).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayCaseCategoryDescription(string UrlPath)
        {
            if (!String.IsNullOrEmpty(UrlPath))
            {
                DataView data = KayCaseCategoryDescriptionDAL.GetDetails(UrlPath).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayCaseCategoryDescription(XmlNode Node)
        {
            SetProperties(Node);
        }
        public KayCaseCategoryDescription(DataRow Row)
        {
            SetProperties(Row);
        }

        // Properties
        private void SetProperties(DataRow Row)
        {            
            Case.Id = int.Parse(Row["Cases_Id"].ToString());
            Case.CaseNumber = Row["Cases_Number"].ToString();
            Case.ParentId = int.Parse(Row["Cases_ParentId"].ToString());
            Case.Plaintiff = Row["Cases_Plaintiff"].ToString();
            Case.Defendant = Row["Cases_Defendant"].ToString();
            Case.Date = Row["Cases_Dates"].ToString();
            Case.CourtType = (KayCourtType)Enum.ToObject(typeof(KayCourtType), int.Parse(Row["Cases_CourtType"].ToString()));
            Case.CaseType = (KayCaseType)Enum.ToObject(typeof(KayCaseType), int.Parse(Row["Cases_CaseType"].ToString()));
            Case.CourtName = Row["Cases_CourtName"].ToString();
            Case.Notes = Row["Cases_Notes"].ToString();
            Case.Appealed = Boolean.Parse(Row["Cases_Appealed"].ToString());
            Case.Ruler = Row["Cases_Ruler"].ToString();
            Case.FullCase = Row["Cases_FullCase"].ToString();
            Case.Recycled = Boolean.Parse(Row["Cases_Recycled"].ToString());

            string test = (Row["Categories_Id"].ToString());
            Category.Id = int.Parse(Row["Categories_Id"].ToString());
            Category.ParentId = int.Parse(Row["Categories_ParentId"].ToString());
            Category.Type = (KayCategoryType)Enum.ToObject(typeof(KayCategoryType), int.Parse(Row["Categories_Type"].ToString())); 
            Category.Title = Row["Categories_Title"].ToString();
            Category.Recycled = Boolean.Parse(Row["Cases_Recycled"].ToString());

            Id = int.Parse(Row["CaseCategoryDescriptions_Id"].ToString());
            Description = Row["CaseCategoryDescriptions_Description"].ToString();           
        }
        private void SetProperties(XmlNode Node)
        {            
        }

        // List
        public static DataView List()
        {
            return KayCaseCategoryDescriptionDAL.GetList().DefaultView;
        }
        public static DataView LiveList()
        {
            return KayCaseCategoryDescriptionDAL.GetLiveList().DefaultView;
        }
        public static DataView Recent(int Count)
        {
            return KayCaseCategoryDescriptionDAL.GetRecentList(Count).DefaultView;
        }
        public static DataView ListByCase(int CaseId)
        {
            return KayCaseCategoryDescriptionDAL.GetListByCase(CaseId).DefaultView;
        }
        public static DataView ListByCategory(int CategoryId)
        {
            return KayCaseCategoryDescriptionDAL.GetListByCategory(CategoryId).DefaultView;
        }
        //search
        public static DataView Find(String Keywords)
        {
            return KayCaseCategoryDescriptionDAL.Find(Keywords).DefaultView;
        }
        public static DataView Find(String Keywords, KayCaseType Type)
        {
            return KayCaseCategoryDescriptionDAL.Find(Keywords, (int)Type).DefaultView;
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
            Id = KayCaseCategoryDescriptionDAL.Add(Case.Id, Category.Id, Description, UrlPath);

            // Move files
            if (Id > 0) Update();

            // Success
            return Id > 0;
        }

        // Update
        private Boolean Update()
        {
            
            // Update database record
            Boolean Success = KayCaseCategoryDescriptionDAL.Update(Id, Case.Id, Category.Id, Description, UrlPath);

            // Success
            return Success;
        }

        // Delete
        public Boolean Delete()
        {
            if (Id > 0)
            {
                // Update database record
                Boolean Success = KayCaseCategoryDescriptionDAL.Delete(Id);

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
            return KayCaseCategoryDescriptionDAL.UniqueUrlPath(Id, UrlPath);
        }       

        #region XML

        public XmlNode ToXmlNode()
        {
            // Start document
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
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
        public static KayCaseCategoryDescription FromXmlString(String Xml)
        {
            XmlNode node = Utilities.StringToXmlNode(Xml);
            return new KayCaseCategoryDescription(node);
        }

        #endregion
    }    

    /// <summary>
    /// CategoryDescription collection
    /// </summary>
    public class KayCaseCategoryDescriptionCollection : CollectionBase
    {
        // Constructors
        public KayCaseCategoryDescriptionCollection()
        { 
        }
        public KayCaseCategoryDescriptionCollection(DataView Data)
        {
            foreach (DataRowView RowView in Data)
            {
                this.Add(new KayCaseCategoryDescription(RowView.Row));
            }
        }
        public KayCaseCategoryDescriptionCollection(XmlDocument Xml)
        {
            foreach (XmlNode Node in Xml.DocumentElement)
            {
                KayCaseCategoryDescription Object = new KayCaseCategoryDescription(Node);
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
            writer.WriteStartElement("cases");

            // Loop through items
            foreach (KayCaseCategoryDescription CategoryDescription in this)
            {
                String node = CategoryDescription.ToXmlNode().OuterXml;
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
        public static KayCaseCollection FromXmlString(String Xml)
        {
            XmlDocument doc = Utilities.StringToXmlDocument(Xml);
            return new KayCaseCollection(doc);
        }

        #endregion

        // Built-in methods
        public KayCaseCategoryDescription this[int index]
        {
            get { return (KayCaseCategoryDescription)List[index]; }
            set { List[index] = value; }
        }
        public int Add(KayCaseCategoryDescription value)
        {
            return List.Add(value);
        }
        public int IndexOf(KayCaseCategoryDescription value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, KayCaseCategoryDescription value)
        {
            List.Insert(index, value);
        }
        public void Remove(KayCaseCategoryDescription value)
        {
            List.Remove(value);
        }
        public bool Contains(KayCaseCategoryDescription value)
        {
            return List.Contains(value);
        }
        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayCaseCategoryDescription"))
                throw new ArgumentException("value must be of type KayCaseCategoryDescription", "value");
        }
        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayCaseCategoryDescription"))
                throw new ArgumentException("value must be of type KayCaseCategoryDescription.", "value");
        }
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("Kay.BLL.KayCaseCategoryDescription"))
                throw new ArgumentException("newValue must be of type KayCaseCategoryDescription.", "newValue");
        }
        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayCaseCategoryDescription"))
                throw new ArgumentException("value must be of type KayCaseCategoryDescription.");
        }
    }
}
