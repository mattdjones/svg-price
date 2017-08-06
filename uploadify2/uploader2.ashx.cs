using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace priceSvg.uploadify2
{
    /// <summary>
    /// Summary description for uploader2
    /// </summary>
    public class uploader2 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;

            string fPath = context.Server.MapPath("../imgLibrary");

            if (null != context.Request.Files["Filedata"])
            {
                HttpPostedFile myFile = context.Request.Files["Filedata"];
                int nFileLen = myFile.ContentLength;
                byte[] myData = new byte[nFileLen];
                myFile.InputStream.Read(myData, 0, nFileLen);
                System.IO.FileStream newFile = new System.IO.FileStream(fPath + @"\" + myFile.FileName, System.IO.FileMode.Create);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();

                context.Response.Write(myFile.FileName);
            }
            else
            {
                context.Response.Write("Failed to upload file.");
            }


            
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