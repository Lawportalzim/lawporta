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
    /// User
    /// </summary>
    public class KayUser
    {
        // Properties
        public int Id = 0;
        public String FirstName = String.Empty;
        public String LastName = String.Empty;
        public String FullName
        {
            get
            {
                if (String.IsNullOrEmpty(FirstName) && String.IsNullOrEmpty(LastName) && !String.IsNullOrEmpty(EmailAddress))
                {
                    return EmailAddress.Split('@')[0];
                }
                return FirstName + " " + LastName;
            }
        }
        public String EmailAddress = String.Empty;
        public String Telephone = String.Empty;
        public KayCompany Company = new KayCompany();
        public KayAddress Address = new KayAddress();
        public String Password = String.Empty;
        public int Groups = (int)KayUserGroups.Guests;
        public DateTime ExpiryDate = DateTime.Now; 
        public DateTime StartDate = DateTime.Now;
        public DateTime LastSeen = DateTime.Now;
        public bool Online = false;
        public bool Active = false;
        public Boolean SuperUser
        {
            get
            {
                return (Groups & (int)KayUserGroups.SuperUsers) > 0;
            }
        }
        public Boolean Administrator
        {
            get
            {
                return (Groups & (int)KayUserGroups.Administrators) > 0;
            }
        }
        public Boolean Member
        {
            get
            {
                return (Groups & (int)KayUserGroups.Members) > 0;
            }
        }

        // Constructor
        public KayUser()
        {
        }
        public KayUser(String EmailAddress)
        {
            if (!String.IsNullOrEmpty(EmailAddress))
            {
                DataView data = KayUserDAL.GetDetails(EmailAddress).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayUser(int Id)
        {
            if (Id > 0)
            {
                DataView data = KayUserDAL.GetDetails(Id).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayUser(XmlNode Node)
        {
            SetProperties(Node);   
        }
        public KayUser(DataRow Row)
        {
            SetProperties(Row);
        }

        // Properties
        public void SetProperties(DataRow Row)
        {
            Id = int.Parse(Row["Users_Id"].ToString());
            FirstName = Row["Users_FirstName"].ToString();
            LastName = Row["Users_LastName"].ToString();
            EmailAddress = Row["Users_EmailAddress"].ToString();
            Telephone = Row["Users_Telephone"].ToString();
            
            Company.Id = int.Parse(Row["Companies_Id"].ToString());
            Company.Name = Row["Companies_Name"].ToString();
            Company.EmailAddress = Row["Companies_EmailAddress"].ToString();
            Company.Telephone = Row["Companies_Telephone"].ToString();
            Company.ContactPerson = Row["Companies_ContactPerson"].ToString();
            Company.Address = KayAddress.FromXmlString(Row["Companies_Address"].ToString());
            Company.NumberOfAccounts = int.Parse(Row["Companies_NumberOfAccounts"].ToString());

            Address = KayAddress.FromXmlString(Row["Users_Address"].ToString());
            Password = Utilities.DecryptText(Row["Users_Password"].ToString(), Id.ToString());
            Groups = int.Parse(Row["Users_Groups"].ToString());
            ExpiryDate =  DateTime.Parse(Row["Users_ExpiryDate"].ToString());
            Online = Boolean.Parse(Row["Users_Online"].ToString());
            Active = bool.Parse(Row["Users_Status"].ToString());
            StartDate = DateTime.Parse(Row["Users_StartDate"].ToString());
            LastSeen = DateTime.Parse(Row["Users_LastSeen"].ToString());
        }
        public void SetProperties(XmlNode Node)
        {
            /*
            Id = int.Parse(Node.ChildNodes[0].FirstChild.Value);
            FirstName = Node.ChildNodes[1].FirstChild.Value;
            LastName = Node.ChildNodes[2].FirstChild.Value;
            EmailAddress = Node.ChildNodes[3].FirstChild.Value;
            Telephone = Node.ChildNodes[4].HasChildNodes ? Node.ChildNodes[4].FirstChild.Value : String.Empty;
            Company = Node.ChildNodes[5].HasChildNodes ? Node.ChildNodes[5].FirstChild.Value : String.Empty;
            Address = new KayAddress(Node.ChildNodes[6]);
            Password = Utilities.DecryptText(Node.ChildNodes[7].FirstChild.Value, Id.ToString());
            Groups = int.Parse(Node.ChildNodes[8].FirstChild.Value);*/
        }

        // List
        public static DataView List()
        {
            return KayUserDAL.GetList().DefaultView;
        }
        public static DataView List(KayCompany Company)
        {
            return KayUserDAL.GetList(Company.Id).DefaultView;
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
            Id = KayUserDAL.Add(FirstName, LastName, EmailAddress, Telephone, Company.Id, Address.ToXmlString(), Utilities.EncryptText(Password), Groups, ExpiryDate, Online, Active, StartDate, LastSeen);

            // Encrypt password using salt
            if (Id > 0) Update();

            // Success
            return Id > 0;
        }

        // Update
        private Boolean Update()
        {
            // Update database record
            Boolean Success = KayUserDAL.Update(Id, FirstName, LastName, EmailAddress, Telephone, Company.Id, Address.ToXmlString(), Utilities.EncryptText(Password, Id.ToString()), Groups, ExpiryDate, Online, Active, StartDate, LastSeen);

            // Success
            return Success;
        }

        // Delete
        public Boolean Delete()
        {
            Boolean success = false;
            if (Id > 0)
            {
                success = KayUserDAL.Delete(Id);
            }
            return success;
        }

        // Unique
        public Boolean UniqueEmailAddress()
        {
            return KayUserDAL.UniqueEmailAddress(Id, EmailAddress);
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
            writer.WriteAttributeString("title", FullName);
            writer.WriteElementString("id", Id.ToString());
            writer.WriteElementString("firstname", FirstName);
            writer.WriteElementString("lastname", LastName);
            writer.WriteElementString("emailaddress", EmailAddress);
            writer.WriteElementString("telephone", Telephone);
            writer.WriteElementString("company", Company.Name);
            writer.WriteRaw(Address.ToXmlString());
            writer.WriteElementString("password", Utilities.EncryptText(Password));
            writer.WriteElementString("groups", Groups.ToString());
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

        public static KayUser FromXmlString(String Xml)
        {
            XmlNode node = Utilities.StringToXmlNode(Xml);
            return new KayUser(node);
        }

        #endregion

        #region Authentication

        // Login
        public static int Login(String EmailAddress, String Password)
        {
            KayUser kayUser = new KayUser(EmailAddress);
            bool flag = false;
            if (kayUser.Id > 0 && Password == kayUser.Password && kayUser.SuperUser)
                flag = true;
            if (kayUser.Id > 0 && Password == kayUser.Password && (!kayUser.Online && kayUser.Active) && kayUser.ExpiryDate >= DateTime.Now)
                flag = true;
            TimeSpan timeSpan;
            int num;
            if (kayUser.Id > 0 && Password == kayUser.Password && kayUser.Online)
            {
                timeSpan = DateTime.Now - kayUser.LastSeen;
                num = timeSpan.TotalMinutes <= 3.0 ? 1 : 0;
            }
            else
                num = 1;
            if (num == 0)
                flag = true;
            timeSpan = DateTime.Now - kayUser.LastSeen;
            double totalMinutes = timeSpan.TotalMinutes;
            if (kayUser.Id < 1)
                return 0;
            if (flag)
            {
                HttpContext.Current.Session.Remove("CurrentUser");
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("Kay_auth")
                {
                    Value = Utilities.EncryptText(kayUser.Id.ToString()),
                    Expires = DateTime.Now.AddMinutes(30.0),
                    Path = "/"
                });
                kayUser.Online = true;
                kayUser.LastSeen = DateTime.Now;
                kayUser.Save();
                return 1;
            }
            if (kayUser.Id > 0 && Password != kayUser.Password)
                return 0;
            if (kayUser.Online)
                return 2;
            if (kayUser.ExpiryDate < DateTime.Now)
                return 3;
            return !kayUser.Active ? 4 : 0;
        }

        // Logout
        public static void Logout()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Kay_auth"];
            if (cookie != null)
            {
                KayUser kayUser = new KayUser(int.Parse(Utilities.DecryptText(cookie.Value)));
                kayUser.Online = false;
                kayUser.LastSeen = DateTime.Now;
                if (kayUser.Id > 0)
                    kayUser.Save();
                cookie.Value = string.Empty;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            HttpContext.Current.Session.Abandon();
        }

        // Send registration email
        /*public Boolean SendRegistrationNotification()
        {
            // Create e-mail
            String To = EmailAddress;
            //String From = Config.AdministratorEmail;
            String Subject = String.Format("Welcome to {0}", Config.ApplicationName);
            String Body = "";
            Body = Body.Replace("{First name}", FirstName);
            Body = Body.Replace("{Last name}", LastName);
            Body = Body.Replace("{Name}", FullName);
            Body = Body.Replace("{E-mail}", EmailAddress);
            Body = Body.Replace("{Telephone}", Telephone);
            Body = Body.Replace("{Password}", Password);

            // Send e-mail
            return true;//Utilities.SendEmail(To, From, Subject, Body);
        }*/

        // Send password
        public Boolean SendPassword()
        {
            // Create e-mail
            String To = EmailAddress;            
            String Subject = "Forgotten password";
            String Body = "";
            Body = Body.Replace("{First name}", FirstName);
            Body = Body.Replace("{Last name}", LastName);
            Body = Body.Replace("{Password}", Password);

            // Send e-mail
            return true; //Utilities.SendEmail(To, From, Subject, Body);
        }
        public static Boolean SendPassword(String EmailAddress)
        {
            KayUser user = new KayUser(EmailAddress);
            if (user.Id == 0) return false;
            return user.SendPassword();
        }

        #endregion
    }

    /// <summary>
    /// Groups
    /// </summary
    public enum KayUserGroups
    {
        Guests = 0,
        SuperUsers = 1,
        Administrators = 2,
        Members = 4
    }

    /// <summary>
    /// User collection
    /// </summary>
    public class KayUserCollection : CollectionBase
    {
        // Constructors
        public KayUserCollection()
        {
        }
        public KayUserCollection(DataView Data)
        {
            foreach (DataRowView RowView in Data)
            {
                this.Add(new KayUser(RowView.Row));
            }
        }
        public KayUserCollection(XmlDocument Xml)
        {
            foreach (XmlNode Node in Xml.DocumentElement)
            {
                KayUser User = new KayUser(Node);
                this.Add(User);
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
            foreach (KayUser User in this)
            {
                String node = User.ToXmlNode().OuterXml;
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

        public static KayUserCollection FromXmlString(String Xml)
        {
            XmlDocument doc = Utilities.StringToXmlDocument(Xml);
            return new KayUserCollection(doc);
        }

        #endregion

        // Built-in methods
        public KayUser this[int index]
        {
            get { return (KayUser)List[index]; }
            set { List[index] = value; }
        }
        public int Add(KayUser value)
        {
            return List.Add(value);
        }
        public int IndexOf(KayUser value)
        {
            return List.IndexOf(value);
        }
        public void Insert(int index, KayUser value)
        {
            List.Insert(index, value);
        }
        public void Remove(KayUser value)
        {
            List.Remove(value);
        }
        public bool Contains(KayUser value)
        {
            return List.Contains(value);
        }
        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayUser"))
                throw new ArgumentException("value must be of type KayUser", "value");
        }
        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayUser"))
                throw new ArgumentException("value must be of type KayUser.", "value");
        }
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("Kay.BLL.KayUser"))
                throw new ArgumentException("newValue must be of type KayUser.", "newValue");
        }
        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("Kay.BLL.KayUser"))
                throw new ArgumentException("value must be of type KayUser.");
        }
    }

    /// <summary>
    /// Import users
    /// </summary>   
}
