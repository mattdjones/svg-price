using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SVGLib;
using System.Drawing;

namespace priceSvg
{
    public class templateSize
    {
        public string txtTemplateSize;
        public bool errTemplate = false;

        public int detectTemplate(string docPath)
        {
            
           // XDocument test = XDocument.Load(docPath);           

           // IEnumerable<XElement> users = (from el in test.Root.Descendants() where (string)el.Attribute("id") == "Your_Designs" select el);

           // XElement element = users.First();
           // string mc = element.Attribute("class").Value.ToString();


            // another change 15 june 2012 now using fill value from nested path in group with id = borders 
            string pathFill = "";
            SvgDoc myDoc = new SvgDoc();
            myDoc.LoadFromFile(docPath);
            SVGLib.SvgElement elBorders = myDoc.GetSvgElement("Borders");
            if (elBorders != null)
            {
                if (elBorders.getChild() != null)
                {
                    SvgElement pathBorder = elBorders.getChild();
                    pathData myPath = new pathData((SvgPath)pathBorder);
                    pathFill = myPath.fillColor;
                    pathBorder = null;
                    myPath = null;
                }
            }
            else
            {
                // check if is an inkscape 
                //check inkscape added group labled "Borders"
                // get the id by searching for the label then us the ID with getSvgElement
                inkscapeGroupFix gf = new inkscapeGroupFix();
                string groupId = gf.getgroupId("Borders", docPath, "Borders");
                if (groupId == "")
                {
                    //show template error - not recognised
                }
                else { 
                    //get the fill colour
                    elBorders = myDoc.GetSvgElement(groupId);
                    SvgElement pathBorder = elBorders.getChild();
                    pathData myPath = new pathData((SvgPath)pathBorder);
                    string pathStyle = myPath.style;
                    extractStyle rStyle = new extractStyle();
                    rStyle.getStyle(pathStyle);
                    pathFill = rStyle.fillColour;
                    
                    //convert to RGB values
                    Color htmlPath = ColorTranslator.FromHtml(pathFill);
                    pathFill = Convert.ToString(htmlPath.R) + Convert.ToString(htmlPath.G) + Convert.ToString(htmlPath.B);

                    rStyle = null;
                    pathBorder = null;
                    myPath = null;

                }
                myDoc = null;
            }




            //IEnumerable<XElement> users = test.Root.Descendants();

            //string viewBox = svgTree.Attribute("viewBox").Value.ToString();



            // chang to get the fill colour from the borders child path element TODO


           // pathData myPath = new pathData((SvgPath)elBorders.getChild());
           // myShapes shapeData = new myShapes();

            //string pathData = myPath.shapeData;
            //double length = shapeData.getPathLength(pathData);
            //int absLength = Convert.ToInt32(Math.Round(length));            

            //myDoc = null;
           // elBorders = null;
            //myPath = null;
            //shapeData = null;
            //svgTree = null;



            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["zapCartDb"].ConnectionString);
            myConn.Open();

            //SqlCommand myCmd = new SqlCommand("SELECT * FROM tbl_templateSizes where viewBox = @viewBox");
            //SqlCommand myCmd = new SqlCommand("SELECT * FROM tbl_templateSizes where templateSizeText = @viewBox");
            SqlCommand myCmd = new SqlCommand("SELECT * FROM tbl_templateSizes where borders = @borders");
            myCmd.Connection = myConn;
            SqlParameter myParam = new SqlParameter();
            myParam.ParameterName = "@borders";
            //myParam.Value = viewBox;
            myParam.Value = pathFill;
            myCmd.Parameters.Add(myParam);

            SqlDataReader myReader = null;

            myReader = myCmd.ExecuteReader();

            int templateSizeId =0;

            while (myReader.Read())
            {
                templateSizeId = (int)myReader["templateSizeId"];
                txtTemplateSize = (string)myReader["templateSizeText"];
                               
            }

            myReader = null;
            myConn.Close();
            myCmd = null;
            myConn = null;


            return templateSizeId;
            
            

        }
    }
}