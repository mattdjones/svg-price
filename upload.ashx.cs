﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace priceSvg
{
    /// <summary>
    /// handler for Plupload
    /// </summary>
    public class upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int chunk = context.Request["chunk"] != null ? int.Parse(context.Request["chunk"]) : 0;
            string fileName = context.Request["name"] != null ? context.Request["name"] : string.Empty;

            HttpPostedFile fileUpload = context.Request.Files[0];

            var uploadPath = context.Server.MapPath(@"imgLibrary");
            using (var fs = new FileStream(Path.Combine(uploadPath, fileName), chunk == 0 ? FileMode.Create : FileMode.Append))
            {
                var buffer = new byte[fileUpload.InputStream.Length];
                fileUpload.InputStream.Read(buffer, 0, buffer.Length);

                fs.Write(buffer, 0, buffer.Length);
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("Success");

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