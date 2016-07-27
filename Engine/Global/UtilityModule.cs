using System;
using System.Data;
using System.Text;
using System.Web;
using System.IO.Compression;
using System.IO;
using System.Web.Caching;
using System.Xml;
using System.Text.RegularExpressions;
using Kay.BLL;

namespace Kay.Global
{
    /// <summary>
    /// Adapted form: http://blogs.ugidotnet.org/kfra/archive/2006/10/04/50003.aspx
    /// </summary>
    public class UtilityModule : IHttpModule
    {
        // Properties
        private HttpContext Context;
        private HttpRequest Request;
        private HttpResponse Response;

        // Init
        public void Init(HttpApplication Context)
        {
            Context.PostResolveRequestCache += new EventHandler(this.PostResolveRequestCache);
        }

        // Dispose
        public void Dispose()
        {
        }

        // Check the request
        private void PostResolveRequestCache(Object sender, EventArgs e)
        {
            // Set properties
            Context = HttpContext.Current;
            Request = Context.Request;
            Response = Context.Response;
        }     
       
        public static byte[] GZipMemory(String Input)
        {
            return GZipMemory(Encoding.UTF8.GetBytes(Input));
        }
        public static byte[] GZipMemory(byte[] Buffer)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream GZip = new GZipStream(ms, CompressionMode.Compress);
            GZip.Write(Buffer, 0, Buffer.Length);
            GZip.Close();
            byte[] Result = ms.ToArray();
            ms.Close();
            return Result;
        }
    }
}
