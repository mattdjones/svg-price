using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace priceSvg
{
    public partial class path : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {




            Process p = new Process();
            p.StartInfo.FileName = @"C:\Python27\python.exe";
            p.StartInfo.Arguments = @"C:\Program Files\Inkscape\share\extensions\measure.py";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
            p.Close();

        }

        private static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
        if (outLine.Data != null)
        Console.Out.WriteLine(outLine.Data.ToString());
        }
    }
}