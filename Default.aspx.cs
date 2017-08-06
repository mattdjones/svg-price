using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SVGLib;
using System.Text;
using System.Collections;



namespace priceSvg
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {


            string testPath = HttpContext.Current.Server.MapPath(TextBox2.Text);

           SvgDoc myDoc = new SvgDoc();

           bool fileOk = myDoc.LoadFromFile(testPath);
            
           //look for <g id="Your_Designs">
           SVGLib.SvgElement elYourDesigns = myDoc.GetSvgElement("Your_Designs");
           
           List<shapePrice> priceArray = AddFromSvg(elYourDesigns);
           zapQuote newQuote = new zapQuote(zapConstants.mAcrylic3mm, 1, 1);
           
           // StringBuilder myString = new StringBuilder();
           
            
           foreach (shapePrice i in priceArray)
           {

              // myString.Append("zap type: " + i.zapType + ", area: " + Convert.ToString( i.area) + ", length: " + Convert.ToString(i.length) + ", x: " + Convert.ToString(i.xpos) + ", y: " + Convert.ToString(i.ypos) + ",\r\n other: " + i.otherInfo + "\r\n");

               newQuote.appendShapeData(i.length, i.area, i.zapType, i.height, 0, 0);

               //myString.Append("\r\n" + i.otherInfo);

           }

           newQuote.calcTotalTime(zapConstants.mAcrylic3mm, 1, 1);


           lblareafill.Text = newQuote.totalAreaFill.ToString();
           lblblightfillcount.Text = newQuote.lightFillCount.ToString();
           lblbtotalengravelength.Text = newQuote.totalLengthEngrave.ToString();
           lblbtotalheavyengrave.Text = newQuote.totalHeavyEngrave.ToString();
           lblcutcount.Text = newQuote.cutCount.ToString();
           lblcuttime.Text = newQuote.cutTime.ToString();
           lblengravetime.Text = newQuote.engraveTime.ToString();
           lblfilltime.Text = newQuote.fillTime.ToString();
           lblheavyengravecount.Text = newQuote.heavyEngraveCount.ToString();
           lblheavyfillarea.Text = newQuote.totalHeavyFill.ToString();
           lblheavyfillcount.Text = newQuote.heavyFillCount.ToString();
           lbllightengravecount.Text = newQuote.lightEngraveCount.ToString();
           lbllightfillarea.Text = newQuote.totalLightFill.ToString();
           lblmedengravecount.Text = newQuote.medEngraveCount.ToString();
           lblmedfillarea.Text = newQuote.totalMedFill.ToString();
           lblmedfillcount.Text = newQuote.medFillCount.ToString();
           lbltotalCutLength.Text = newQuote.totalCutLength.ToString();
           lbltotallightengrave.Text = newQuote.totalLengthEngrave.ToString();
           lbltotalmedengrave.Text = newQuote.totalMedEngrave.ToString();
           lbltotaltime.Text = newQuote.totalTime.ToString();


           //TextBox1.Text = myString.ToString();
            

            //errors
            //errors myHandler = new errors();
            //myHandler.errFileSize(testPath);

          // TextBox1.Text = myHandler.currentError;



           //string gPath = @"<path d=""M 100 100 L 300 100 L 200 300 z"" fill=""red"" stroke=""blue"" stroke-width=""3"" />";


           

        }






        //public List<shapePrice> AddFromSvg(SVGLib.SvgElement ele)
        //{                       
        //    List<shapePrice> allShapes = new List<shapePrice>();            

        //    while (ele != null)
        //    {
        //        getElementData tData = new getElementData(ele);                    
        //        allShapes.Add(new shapePrice(tData.zapType, tData.length, tData.area, tData.xpos, tData.ypos, tData.userErrorMsg, tData.shapeHeight));
        //        tData = null;

        //        SVGLib.SvgElement child = ele.getChild();
        //        while (child != null)
        //        {
        //            getElementData cData = new getElementData(child);
        //            allShapes.Add(new shapePrice(cData.zapType, cData.length, cData.area, cData.xpos, cData.ypos, cData.userErrorMsg, cData.shapeHeight));

        //            if (child.getElementType() == SvgElement._SvgElementType.typeGroup)
        //            {
        //                SVGLib.SvgElement groupChild = child.getChild();
        //                while (groupChild != null)
        //                {
        //                    getElementData groupcHildData = new getElementData(groupChild);
        //                    allShapes.Add(new shapePrice(groupcHildData.zapType, groupcHildData.length, groupcHildData.area, groupcHildData.xpos, groupcHildData.ypos, groupcHildData.userErrorMsg, groupcHildData.shapeHeight));
        //                    if (groupcHildData.userError)
        //                    {
        //                        // throw error
        //                    }
        //                    groupcHildData = null;
        //                    AddFromSvg(groupChild);
        //                    groupChild = groupChild.getNext();
        //                }
        //            }

        //            cData = null;

        //            AddFromSvg(child);
        //            child = child.getNext();
        //        }
        //        child = null;
        //        tData = null;
        //        ele = ele.getNext();
        //    }

        //    ele = null;
        //    return allShapes;

        //}


    }
}