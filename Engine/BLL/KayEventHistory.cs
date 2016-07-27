using System;
using System.Data;
using System.Text;
using System.Web;
using System.Xml;
using Kay.DAL;

namespace Kay.BLL
{
    /// <summary>
    ///	Event history
    /// </summary>
    public class KayEventHistory
    {
        // Properties
        public Double Id = 0;
        public String Title = String.Empty;
        public String Data = String.Empty;
        public DateTime Date = DateTime.MinValue;
        public KayEventHistoryType Type = KayEventHistoryType.Log;

        // Constructor
        public KayEventHistory()
        {
        }
        public KayEventHistory(Double Id)
        {
            if (Id > 0)
            {
                DataView data = KayEventHistoryDAL.GetDetails(Id).DefaultView;
                if (data.Count == 1) SetProperties(data[0].Row);
            }
        }
        public KayEventHistory(DataRow Row)
        {
            SetProperties(Row);
        }

        // Properties
        public void SetProperties(DataRow Row)
        {
            Id = Double.Parse(Row["Id"].ToString());
            Title = Row["Title"].ToString();
            Data = Row["Data"].ToString();
            Date = DateTime.Parse(Row["Date"].ToString());
            Type = (KayEventHistoryType)Enum.ToObject(typeof(KayEventHistoryType), int.Parse(Row["Type"].ToString()));
        }

        // List
        public static DataView List()
        {
            return KayEventHistoryDAL.GetList().DefaultView;
        }
        public static DataView ListByType(KayEventHistoryType Type)
        {
            return KayEventHistoryDAL.GetListByType((int)Type).DefaultView;
        }
        public static DataView TaskList(int Count)
        {
            return KayEventHistoryDAL.GetTaskList(Count).DefaultView;
        }

        // Add event
        public static Double Error(String Title, Exception exception)
        {
            return AddEvent(Title, exception.Message + Environment.NewLine + exception.StackTrace, KayEventHistoryType.Error);
        }
        public static Double Log(String Title, String Data)
        {
            return AddEvent(Title, Data, KayEventHistoryType.Log);
        }
        public static Double AddEvent(String Title, String Data, KayEventHistoryType Type)
        {
            return KayEventHistoryDAL.Add(Title, Data, (int)Type);
        }

        // Email event
        public static Double EmailSuccess(String To, String From, String Subject, String Message)
        {
            return Email(To, From, Subject, Message, true, String.Empty);
        }
        public static Double EmailFailure(String To, String From, String Subject, String Message, Exception Error)
        {
            return Email(To, From, Subject, Message, false, Error.Message);
        }
        private static Double Email(String To, String From, String Subject, String Message, Boolean Success, String ErrorMessage)
        {
            KayEventHistoryEmail email = new KayEventHistoryEmail();
            email.To = To;
            email.From = From;
            email.Subject = Subject;
            email.Message = Message;
            email.Result = Success ? "Mail sent successfully" : ErrorMessage;
            
            // Update database
            return AddEvent(Subject, email.ToXmlNode().OuterXml, KayEventHistoryType.Email);
        }

        // Delete
        public Boolean Delete()
        {
            return KayEventHistoryDAL.Delete(Id);
        }
        public static Boolean Clear(KayEventHistoryType Type)
        {
            return KayEventHistoryDAL.Clear((int)Type);
        }
    }

    /// <summary>
    /// Event history type
    /// </summary>
    public enum KayEventHistoryType
    {
        Error = 1, 
        Log = 2, 
        Task = 3,
        Email = 4
    }

    /// <summary>
    ///	Event history e-mail
    /// </summary>
    public class KayEventHistoryEmail
    {
        // Properties
        public String To = String.Empty;
        public String From = String.Empty;
        public String Subject = String.Empty;
        public String Message = String.Empty;
        public DateTime Date = DateTime.Now;
        public String Result = String.Empty;

        // Constructor
        public KayEventHistoryEmail()
        {
        }
        public KayEventHistoryEmail(XmlNode Node)
        {
            To = Node.ChildNodes[0].InnerText;
            From = Node.ChildNodes[1].InnerText;
            Subject = Node.ChildNodes[2].InnerText;
            Message = HttpUtility.HtmlDecode(Node.ChildNodes[3].InnerText);
            Date = DateTime.Parse(Node.ChildNodes[4].InnerText);
            Result = Node.ChildNodes[5].InnerText;
        }

        // XML
        public XmlNode ToXmlNode()
        {
            // Start document
            StringBuilder xml = new StringBuilder();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(xml);

            // Object node
            writer.WriteStartElement("item");
            writer.WriteElementString("to", To);
            writer.WriteElementString("from", From);
            writer.WriteElementString("subject", Subject);
            writer.WriteElementString("message", HttpUtility.HtmlEncode(Message));
            writer.WriteElementString("date", Date.ToString());
            writer.WriteElementString("result", Result);
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
}
