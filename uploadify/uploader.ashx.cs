using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priceSvg.uploadify
{
    /// <summary>
    /// Summary description for uploader
    /// </summary>
    public class uploader : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string fPath = context.Server.MapPath("../imgLibrary");

                HttpPostedFile svgFile = context.Request.Files["FileData"];

                svgFile.SaveAs(fPath + @"\" + svgFile.FileName);

                context.Response.ContentType = "text/plain";
                context.Response.Write(svgFile.FileName);
            }
            catch(Exception ex) 
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("1");
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