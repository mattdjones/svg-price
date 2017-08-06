using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Configuration;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace priceSvg
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

           zapConstants.pythonMethodPath = WebConfigurationManager.AppSettings["pythonAreaPath"].ToString();

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            zapConstants.ipy = Python.CreateRuntime();
            zapConstants.pathCall = zapConstants.ipy.ExecuteFile(zapConstants.pythonMethodPath);

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            //zapConstants.ipy = null;
            //zapConstants.pathCall = null;
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}