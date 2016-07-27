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
    public class KayCase
    {
        // Properties
        public int Id = 0;
        public string CaseNumber;
        public int ParentId = 0;
        public string Plaintiff = string.Empty;
        public string Defendant = string.Empty;
        public string Date = string.Empty;
        public KayCourtType CourtType = KayCourtType.HighCourt;
        public KayCaseType CaseType = KayCaseType.Civil;
        public string CourtName = string.Empty;
        public string Notes = string.Empty;
        public Boolean Appealed = false;
        public string Ruler = string.Empty;
        public string FullCase = string.Empty;
        public Boolean Recycled = false;

        public String RulerUrlPath
        {
            get
            {
                return Utilities.GenerateUrlPath(Ruler);
            }
        }

        public String UrlPath
        {
            get
            {
                return Utilities.GenerateUrlPath(
                    "Cases", CaseType, Id, Plaintiff, Defendant);
            }
        }

        public Boolean Civil
        {
            get
            {
                return CaseType == KayCaseType.Civil;
            }
            set
            {
                CaseType = value ? KayCaseType.Civil : KayCaseType.Criminal;
            }
        }

        private KayCaseCategoryDescriptionCollection _caseCategoryDescriptions;
        public KayCaseCategoryDescriptionCollection CaseCategoryDescriptions
        {
            get
            {
                if (_caseCategoryDescriptions == null)
                {
                    _caseCategoryDescriptions = new KayCaseCategoryDescriptionCollection(KayCaseCategoryDescription.ListByCase(Id));
                }
                return _caseCategoryDescriptions;
            }
            set
            {
                _caseCategoryDescriptions = value;
            }
        }

        // Constructor
        public KayCase()
        {
        }
        public KayCase(int Id)
        {
            if (Id > 0)
            {
                DataView data = KayCaseDAL.GetDetails(Id).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayCase(int Id, Boolean ParentId)
        {
            if (Id > 0)
            {
                DataView data = KayCaseDAL.GetDetailsByParent(Id).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayCase(string UrlPath)
        {
            if (!String.IsNullOrEmpty(UrlPath))
            {
                DataView data = KayCaseDAL.GetDetails(UrlPath).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayCase(XmlNode Node)
        {
            SetProperties(Node);
        }
        public KayCase(DataRow Row)
        {
            SetProperties(Row);
        }

        // Properties
        private void SetProperties(DataRow Row)
        {
            Id = int.Parse(Row["Cases_Id"].ToString());
            CaseNumber = Row["Cases_Number"].ToString();
            ParentId = int.Parse(Row["Cases_ParentId"].ToString());
            Plaintiff = Row["Cases_Plaintiff"].ToString();
            Defendant = Row["Cases_Defendant"].ToString();
            Date = Row["Cases_Dates"].ToString();
            CourtType = (KayCourtType)Enum.ToObject(typeof(KayCourtType), int.Parse(Row["Cases_CourtType"].ToString()));
            CaseType = (KayCaseType)Enum.ToObject(typeof(KayCaseType), int.Parse(Row["Cases_CaseType"].ToString()));
            CourtName = Row["Cases_CourtName"].ToString();
            Notes = Row["Cases_Notes"].ToString();            
            Appealed = Boolean.Parse(Row["Cases_Appealed"].ToString());
            Ruler = Row["Cases_Ruler"].ToString();
            FullCase = Row["Cases_FullCase"].ToString();
            Recycled = Boolean.Parse(Row["Cases_Recycled"].ToString());
        }
        private void SetProperties(XmlNode Node)
        {            
        }

        // List
        public static DataView List()
        {
            return KayCaseDAL.GetList().DefaultView;
        }
        public static DataView List(int Type)
        {
            return KayCaseDAL.GetList(Type).DefaultView;
        }
        public static DataView RecycledList()
        {
            return KayCaseDAL.GetRecycledList().DefaultView;
        }
        public static DataView LiveList()
        {
            return KayCaseDAL.GetLiveList().DefaultView;
        }
        public static DataView Recent(int Count)
        {
            return KayCaseDAL.GetRecentList(Count).DefaultView;
        }
        public static DataView Find(String Keywords)
        {
            return KayCaseDAL.Find(Keywords).DefaultView;
        }
        public static DataView Find(String Keywords, KayCaseType Type)
        {
            return KayCaseDAL.Find(Keywords, (int)Type).DefaultView;
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
            Id = KayCaseDAL.Add(Plaintiff, Defendant, Date, (int)CourtType, CourtName, UrlPath, Notes, (int)CaseType, ParentId, Appealed, Ruler, CaseNumber, FullCase, Recycled, RulerUrlPath);

            // Move files
            if (Id > 0) Update();

            // Success
            return Id > 0;
        }

        // Update
        private Boolean Update()
        {
            
            // Update database record
            Boolean Success = KayCaseDAL.Update(Id, Plaintiff, Defendant, Date, (int)CourtType, CourtName, UrlPath, Notes, (int)CaseType, ParentId, Appealed, Ruler, CaseNumber, FullCase, Recycled, RulerUrlPath);

            // Success
            return Success;
        }

        // Delete
        public Boolean Delete()
        {
            if (Id > 0)
            {
                // Update database record
                Boolean Success = KayCaseDAL.Delete(Id);

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
            return KayCaseDAL.UniqueUrlPath(Id, UrlPath);
        }
        public Boolean UniqueCaseNumber()
        {
            return KayCaseDAL.UniqueCaseNumber(CaseNumber, Id);
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
        public static KayCase FromXmlString(String Xml)
        {
            XmlNode node = Utilities.StringToXmlNode(Xml);
            return new KayCase(node);
        }

        #endregion
    }

    /// <summary>
    /// Case status
    /// </summary>
    public enum KayCourtType : int
    {
        SepremeCourt = 1,
        HighCourt = 2,
        MegistrateCourt = 3
    }
    public enum KayCaseType : int
    {
        Criminal = 1,
        Civil = 2
    }

    /// <summary>
    /// Case collection
    /// </summary>
    public class KayCaseCollection : CollectionBase
    {
        // Constructors
        public KayCaseCollection()
        { 
        }
        public KayCaseCollection(DataView Data)
        {
            foreach (DataRowView RowView in Data)
            {
                this.Add(new KayCase(RowView.Row));
            }
        }
        public KayCaseCollection(XmlDocument Xml)
        {
            foreach (XmlNode Node in Xml.DocumentElement)
            {
                KayCase Object = new KayCase(Node);
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
            foreach (KayCase Case in this)
            {
                String node = Case.ToXmlNode().OuterXml;
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
        public KayCase this[int index]
        {
            get { return (KayCase)List[index]; }
            set { List[index] = value; }
        }
        public int Add(KayCase value)
        {
            return List.Add(value);
        }
        public int IndexOf(KayCase value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, KayCase value)
        {
            List.Insert(index, value);
        }
        public void Remove(KayCase value)
        {
            List.Remove(value);
        }
        public bool Contains(KayCase value)
        {
            return List.Contains(value);
        }
        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayCase"))
                throw new ArgumentException("value must be of type KayCase", "value");
        }
        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayCase"))
                throw new ArgumentException("value must be of type KayCase.", "value");
        }
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("Kay.BLL.KayCase"))
                throw new ArgumentException("newValue must be of type KayCase.", "newValue");
        }
        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayCase"))
                throw new ArgumentException("value must be of type KayCase.");
        }
    }
}
