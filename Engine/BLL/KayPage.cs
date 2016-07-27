using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Xml;
using Kay.Global;
using Kay.DAL;

namespace Kay.BLL
{
    /// <summary>
    /// Page
    /// </summary>
    public class KayPage : IHierarchyData
    {
        // Caching
        private static String _pagesCacheKey = "pages";
        public static DataView _pages
        {
            get
            {
                if (HttpRuntime.Cache[_pagesCacheKey] == null)
                {
                    HttpRuntime.Cache.Add(_pagesCacheKey, KayPage.List(), null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                }
                return (DataView)HttpRuntime.Cache[_pagesCacheKey];
            }
            set
            {
                if (value == null)
                {
                    HttpRuntime.Cache.Remove(_pagesCacheKey);
                }
                else
                {
                    HttpRuntime.Cache[_pagesCacheKey] = value;
                }
            }
        }

        // Properties
        public int Id = 0;
        public int ParentId = 0;
        public KayPageTemplate Template = new KayPageTemplate();
        public String Title = String.Empty;
        public String ShortTitle = String.Empty;
        public String BrowserTitle = String.Empty;
        public String Description = String.Empty;
        public String Keywords = String.Empty;
        private String _content = String.Empty;
        private KayPagePlaceholderCollection content = null;
        public KayPagePlaceholderCollection Content
        {
            get
            {
                if (content == null)
                    content = new KayPagePlaceholderCollection(_content);
                return content;
            }
            set
            {
                content = value;
            }
        }
        public String UrlPath = String.Empty;
        private KayPageStatus Status = KayPageStatus.Live;
        public Boolean Live
        {
            get
            {
                return Status == KayPageStatus.Live;
            }
            set
            {
                Status = value ? KayPageStatus.Live : KayPageStatus.Hidden;
            }
        }
        public Boolean ShowInMenu = true;
        public KayDevice Device = KayDevice.Public;

        // Constructor
        public KayPage()
        {
        }
        public KayPage(int Id)
        {
            if (Id > 0)
            {
                DataView data = KayPageDAL.GetDetails(Id).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayPage(String UrlPath, KayDevice Device)
        {
            if (!String.IsNullOrEmpty(UrlPath))
            {
                // DataView data = KayPageDAL.GetDetails(UrlPath, (int)Device).DefaultView;
                // if (data.Count == 1) SetProperties(data[0].Row);

                // Get From copy of Pages DataView in Cache
                DataView Pages = new DataView(_pages.Table);
                Pages.RowFilter = String.Format("UrlPath = '{0}' AND Device = {1} AND TemplateTitle <> 'Shortcut'", UrlPath, (int)Device);
                if (Pages.Count == 1)
                {
                    SetProperties(Pages[0].Row);
                }      
            }
        }
        public KayPage(DataRow Row)
        {
            SetProperties(Row);
        }
        public KayPage(XmlNode Node)
        {
            SetProperties(Node);
        }

        // Properties
        private void SetProperties(DataRow Row)
        {
            Id = int.Parse(Row["Id"].ToString());
            try { ParentId = int.Parse(Row["ParentId"].ToString()); }
            catch { ParentId = 0; }
            Template.Id = int.Parse(Row["TemplateId"].ToString());
            Template.Title = Row["TemplateTitle"].ToString();
            Template.Filepath = Row["TemplateFilepath"].ToString();
            Template.Greedy = (Row["TemplateGreedy"].ToString() == "1");
            Title = Row["Title"].ToString();
            ShortTitle = Row["ShortTitle"].ToString();
            BrowserTitle = Row["BrowserTitle"].ToString();
            Description = Row["Description"].ToString();
            Keywords = Row["Keywords"].ToString();
            _content = Row["Content"].ToString();
            UrlPath = Row["UrlPath"].ToString().ToLower();
            Status = (KayPageStatus)Enum.ToObject(typeof(KayPageStatus), int.Parse(Row["Status"].ToString()));
            ShowInMenu = (Row["ShowInMenu"].ToString() == "1");
            Device = (KayDevice)Enum.ToObject(typeof(KayDevice), int.Parse(Row["Device"].ToString()));
        }
        private void SetProperties(XmlNode Node)
        {
            Id = int.Parse(Node.ChildNodes[0].FirstChild.Value);
            ParentId = int.Parse(Node.ChildNodes[1].FirstChild.Value);
            Template = new KayPageTemplate(Node.ChildNodes[2]);
            Title = Node.ChildNodes[3].FirstChild.Value;
            ShortTitle = Node.ChildNodes[4].HasChildNodes ? Node.ChildNodes[4].FirstChild.Value : String.Empty;
            BrowserTitle = Node.ChildNodes[5].HasChildNodes ? Node.ChildNodes[5].FirstChild.Value : String.Empty;
            Description = Node.ChildNodes[6].HasChildNodes ? Node.ChildNodes[6].FirstChild.Value : String.Empty;
            Keywords = Node.ChildNodes[7].HasChildNodes ? Node.ChildNodes[7].FirstChild.Value : String.Empty;
            Content = new KayPagePlaceholderCollection(Utilities.StringToXmlDocument(Node.ChildNodes[8].OuterXml));
            UrlPath = Node.ChildNodes[9].FirstChild.Value;
            Status = (KayPageStatus)Enum.ToObject(typeof(KayPageStatus), int.Parse(Node.ChildNodes[10].FirstChild.Value));
            ShowInMenu = Node.ChildNodes[11].FirstChild.Value == "true";
            Device = (KayDevice)Enum.ToObject(typeof(KayDevice), int.Parse(Node.ChildNodes[12].FirstChild.Value));
        }

        // List
        public static DataView List()
        {
            return KayPageDAL.GetList().DefaultView;
        }
        public static DataView List(KayDevice Device)
        {
            return KayPageDAL.GetList((int)Device).DefaultView;
        }
        public static DataView Find(String Keywords)
        {
            return KayPageDAL.Find(Keywords).DefaultView;
        }
        public static DataView SitemapList(KayDevice Device)
        {
            return KayPageDAL.GetSitemapList((int)Device).DefaultView;
        }

        // Save
        public Boolean Save()
        {
            // Add/update
            Boolean Success = (Id == 0) ? Add() : Update();

            // Clear cache
            _pages = null;

            // Done
            return Success;
        }

        // Add
        private Boolean Add()
        {
            // Insert database record
            Id = KayPageDAL.Add(ParentId, Template.Id, Title, ShortTitle, BrowserTitle, Description, Keywords, Content.ToXml().OuterXml, UrlPath, (int)Status, ShowInMenu ? 1 : 0, (int)Device);

            // Success
            return Id > 0;
        }

        // Update
        private Boolean Update()
        {
            // Update database record
            Boolean Success = KayPageDAL.Update(Id, ParentId, Template.Id, Title, ShortTitle, BrowserTitle, Description, Keywords, Content.ToXml().OuterXml, UrlPath, (int)Status, ShowInMenu ? 1 : 0, (int)Device);

            // Success
            return Success;
        }

        // Sort order
        public Boolean UpdateOrder(int ReferenceId, String Direction)
        {
            // Update record
            Boolean Success = KayPageDAL.UpdateOrder(Id, ReferenceId, Direction);

            // Done
            return Success;
        }

        // Delete
        public Boolean Delete()
        {
            // Update database record
            Boolean Success = KayPageDAL.Delete(Id);

            // Clear cache
            _pages = null;

           

            // Success
            return Success;
        }

        

       

        #region XML

        public XmlNode ToXmlNode()
        {
            // Start document
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");

            // Object node
            writer.WriteStartElement("page");
            // - attributes (do not remove) -
            writer.WriteAttributeString("id", Utilities.GetRandomPasswordUsingGUID(12));
            writer.WriteAttributeString("title", Title);
            // - properties (change to match properties of data object) -
            writer.WriteElementString("id", Id.ToString());
            writer.WriteElementString("parentid", ParentId.ToString());
            writer.WriteRaw(Template.ToXmlString());
            writer.WriteElementString("title", Title);
            writer.WriteElementString("shorttitle", ShortTitle);
            writer.WriteElementString("browsertitle", BrowserTitle);
            writer.WriteElementString("description", Description);
            writer.WriteElementString("keywords", Keywords);
            writer.WriteRaw(Content.ToXml().DocumentElement.OuterXml);
            writer.WriteElementString("urlpath", UrlPath);
            writer.WriteElementString("status", ((int)Status).ToString());
            writer.WriteElementString("showinmenu", ShowInMenu.ToString().ToLower());
            writer.WriteElementString("platform", ((int)Device).ToString());
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

        public static KayPage FromXmlString(String Xml)
        {
            XmlNode node = Utilities.StringToXmlNode(Xml);
            return new KayPage(node);
        }

        #endregion

        #region IHierarchyData Members

        // Gets an enumeration object that represents all the child 
        // nodes of the current hierarchical node.
        public IHierarchicalEnumerable GetChildren()
        {
            // Call to the local cache for the data
            KayPageList children = new KayPageList();

            // Loop through your local data and find any children
            foreach (DataRowView RowView in _pages)
            {
                KayPage page = new KayPage(RowView.Row);
                if (page.ParentId == this.Id)
                {
                    children.Add(page);
                }
            }

            // Done
            return children;
        }

        // Gets an IHierarchyData object that represents the parent node 
        // of the current hierarchical node.
        public IHierarchyData GetParent()
        {
            // Loop through your local data and report back with the parent
            foreach (DataRowView RowView in _pages)
            {
                KayPage page = new KayPage(RowView.Row);
                if (page.Id == this.ParentId)
                {
                    return page;
                }
            }

            return null;
        }

        // Gets a boolean
        public Boolean HasChildren
        {
            get
            {
                KayPageList children = GetChildren() as KayPageList;
                return children.Count > 0;
            }
        }

        // Gets the hierarchical data node that the object represents.
        public object Item
        {
            get { return this; }
        }

        // Gets the hierarchical path of the node.
        public string Path
        {
            get
            {
                return this.Id.ToString();
            }
        }
        public string Type
        {
            get
            {
                return this.GetType().ToString();
            }
        }

        #endregion
    }

    /// <summary>
    /// Page type
    /// </summary>
    public enum KayPageType : int
    {
        Custom = 1,
        Standard = 2
    }

    /// <summary>
    /// Page platform
    /// </summary>
    public enum KayDevice : int
    {
        Unknown = 0,
        Public = 1,
        Mobile = 2
    }

    /// <summary>
    /// Page status
    /// </summary>
    public enum KayPageStatus : int
    {
        Live = 1,
        Hidden = 2
    }

    /// <summary>
    ///	Page list
    /// </summary>
    public class KayPageList : List<KayPage>, IHierarchicalEnumerable
    {
        public KayPageList()
            : base()
        {
        }
        public KayPageList(DataView Data)
        {
            foreach (DataRowView RowView in Data)
            {
                this.Add(new KayPage(RowView.Row));
            }
        }

        #region IHierarchicalEnumerable Members

        // Returns a hierarchical data item for the specified enumerated item.
        public IHierarchyData GetHierarchyData(object enumeratedItem)
        {
            return enumeratedItem as IHierarchyData;
        }

        #endregion
    }

    /// <summary>
    /// Page promo
    /// </summary>
    public class KayPagePromo
    {
        // Properties
        public String Title = String.Empty;       
        public String Link = String.Empty;

        // Constructor
        public KayPagePromo()
        {
        }
        public KayPagePromo(XmlNode Node)
        {
            SetProperties(Node);
        }

        // Properties
        private void SetProperties(XmlNode Node)
        {
            Title = Node.ChildNodes[0].FirstChild.Value;          
            Link = Node.ChildNodes[2].FirstChild.Value;
        }

        // XML
        public XmlNode ToXmlNode()
        {
            // Start document
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");

            // Object node
            writer.WriteStartElement("PagePromo");
            writer.WriteAttributeString("id", Utilities.GetRandomPasswordUsingGUID(12));
            writer.WriteAttributeString("title", Title);
            writer.WriteElementString("title", Title);            
            writer.WriteElementString("link", Link);
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
    }

    /// <summary>
    /// Page promo collection
    /// </summary>
    public class KayPagePromoCollection : CollectionBase
    {
        // Constructors
        public KayPagePromoCollection()
        {
        }
        public KayPagePromoCollection(XmlDocument Xml)
        {
            foreach (XmlNode Node in Xml.DocumentElement)
            {
                KayPagePromo Object = new KayPagePromo(Node);
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
            writer.WriteStartElement("PagePromos");

            // Loop through items
            foreach (KayPagePromo PagePromo in this)
            {
                String node = PagePromo.ToXmlNode().OuterXml;
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
        public KayPagePromo this[int index]
        {
            get { return (KayPagePromo)List[index]; }
            set { List[index] = value; }
        }
        public int Add(KayPagePromo value)
        {
            return List.Add(value);
        }
        public int IndexOf(KayPagePromo value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, KayPagePromo value)
        {
            List.Insert(index, value);
        }
        public void Remove(KayPagePromo value)
        {
            List.Remove(value);
        }
        public bool Contains(KayPagePromo value)
        {
            return List.Contains(value);
        }
        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayPagePromo"))
                throw new ArgumentException("value must be of type KayPagePromo", "value");
        }
        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayPagePromo"))
                throw new ArgumentException("value must be of type KayPagePromo.", "value");
        }
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("Kay.BLL.KayPagePromo"))
                throw new ArgumentException("newValue must be of type KayPagePromo.", "newValue");
        }
        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayPagePromo"))
                throw new ArgumentException("value must be of type KayPagePromo.");
        }
    }

    /// <summary>
    /// Page content
    /// </summary>
    public class KayPageContent
    {
        // Properties
        public String Title = String.Empty;        
        public String Description = String.Empty;
        public String Link = String.Empty;

        // Constructor
        public KayPageContent()
        {
        }
        public KayPageContent(XmlNode Node)
        {
            SetProperties(Node);
        }

        // Properties
        private void SetProperties(XmlNode Node)
        {
            Title = Node.ChildNodes[0].FirstChild.Value;          
            Description = HttpUtility.HtmlDecode(Node.ChildNodes[2].FirstChild.Value);
            Link = Node.ChildNodes[3].HasChildNodes ? Node.ChildNodes[3].FirstChild.Value : "";
        }

        // XML
        public XmlNode ToXmlNode()
        {
            // Start document
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");

            // Object node
            writer.WriteStartElement("PageContent");
            writer.WriteAttributeString("id", Utilities.GetRandomPasswordUsingGUID(12));
            writer.WriteAttributeString("title", Title);
            writer.WriteElementString("title", Title);           
            writer.WriteElementString("description", HttpUtility.HtmlEncode(Description));
            writer.WriteElementString("link", Link);
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
    }

    /// <summary>
    /// Page content collection
    /// </summary>
    public class KayPageContentCollection : CollectionBase
    {
        // Constructors
        public KayPageContentCollection()
        {
        }
        public KayPageContentCollection(XmlDocument Xml)
        {
            foreach (XmlNode Node in Xml.DocumentElement)
            {
                KayPageContent Object = new KayPageContent(Node);
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
            writer.WriteStartElement("PageContents");

            // Loop through items
            foreach (KayPageContent PageContent in this)
            {
                String node = PageContent.ToXmlNode().OuterXml;
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
        public KayPageContent this[int index]
        {
            get { return (KayPageContent)List[index]; }
            set { List[index] = value; }
        }
        public int Add(KayPageContent value)
        {
            return List.Add(value);
        }
        public int IndexOf(KayPageContent value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, KayPageContent value)
        {
            List.Insert(index, value);
        }
        public void Remove(KayPageContent value)
        {
            List.Remove(value);
        }
        public bool Contains(KayPageContent value)
        {
            return List.Contains(value);
        }
        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayPageContent"))
                throw new ArgumentException("value must be of type KayPageContent", "value");
        }
        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayPageContent"))
                throw new ArgumentException("value must be of type KayPageContent.", "value");
        }
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("Kay.BLL.KayPageContent"))
                throw new ArgumentException("newValue must be of type KayPageContent.", "newValue");
        }
        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayPageContent"))
                throw new ArgumentException("value must be of type KayPageContent.");
        }
    }
}
