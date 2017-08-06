using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace priceSvg
{
    /// <summary>
    /// Summary description for clearForm
    /// </summary>
    public class clearForm : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string cFile = context.Request.Form["cFile"];
            string msg;
            try
            {
                string fPath = context.Server.MapPath("imgLibrary");
                string filePath = fPath + @"\" + cFile;
                if (File.Exists(filePath) == true)
                {
                    File.Delete(filePath);
                }

                msg = "1 " + filePath;
            }
            catch (UnauthorizedAccessException ex)
            {
                msg = "0";
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(msg);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}