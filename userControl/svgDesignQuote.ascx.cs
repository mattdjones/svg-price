 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SVGLib;
using System.Text;
using System.Globalization;



namespace priceSvg.userControl
{
    public partial class svgDesignQuote : System.Web.UI.UserControl
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

            svgQuantity.Attributes.Add("onkeyup", "checkQuantity();");

            //add form check to drop down and select lists
            svgMaterial.Attributes.Add("onChange", "checkForm();");
            svgMm.Attributes.Add("onChange", "checkForm();");
            materialColour.Attributes.Add("onChange", "checkForm();");
            imgClearForm.Attributes.Add("onClick", "clearForm();");


            if (Page.IsPostBack)
            {
                


            }

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //clear errors
            //litErr.Text = "";

            ////check  errors
            //errors errorChecker = new errors();

            //string fPath = Context.Server.MapPath(@"imgLibrary");

            //if (svgTempDoc.HasFile)
            //{
            //    //now using uploadify
            //    //first check file size
            //    errorChecker.errFileSize(svgTempDoc.PostedFile.ContentLength);
            //    if (errorChecker.hasError)
            //    {
            //        //return error and exit code
            //        litErr.Text = errorChecker.currentError + errorChecker.helpUrl;
            //        return;
            //    }

            //    try
            //    {
            //        string fullPath = fPath + @"\" + svgTempDoc.FileName;                    
            //        svgTempDoc.SaveAs(fullPath);
            //        Context.Session.Add("fullPath", fullPath);                    
            //        //show file name and template size
            //        lblSsvgFileName.Text = @"&#8226; " + svgTempDoc.FileName;
            //        btnCalc.Enabled = true;
            //        valsvgMaterial.Enabled = true;
            //        valSvgMm.Enabled = true;                    
            //    }
            //    catch (Exception ex)
            //    {
            //        litErr.Text = ex.Message.ToString();
            //    }                
            //}       

        }

        public string svgError;
        public bool isValidSvgDesign = true;
        public List<shapePrice> allShapes = new List<shapePrice>();
        public List<int> toAdd = new List<int>();
        public List<Point> rasterPoints = new List<Point>();
        public double rasterExtraArea = 0;
        public double extraHeight = 0;
        //public StringBuilder db = new StringBuilder(); 

        public void AddFromSvg2(SvgElement ele)
        {
            while (ele != null)
            {
                //do stuff

                if (ele.getElementType() != SvgElement._SvgElementType.typeGroup)
                {
                    if(toAdd.Contains(ele.getInternalId()) != true) {
                        //db.Append(ele.GetTagXml() + "\r\n");
                        //add to object list 
                        getElementData tData = new getElementData(ele);
                        allShapes.Add(new shapePrice(tData.zapType, tData.length, tData.area, tData.xpos, tData.ypos, tData.userErrorMsg, tData.shapeHeight, tData.rasterArea));
                        if (tData.userError)
                        {
                            // throw error
                            svgError = tData.userErrorMsg + "<p>(" + tData.userErrorElement + ")";
                            isValidSvgDesign = false;
                            //break;
                            return;
                        }

                        //check if there are raster points to add
                        if (tData.zapType == zapConstants.rasterLine)
                        {
                            if (tData.pointList.Count > 0) 
                            {
                                rasterPoints.AddRange(tData.pointList);
                            }
                        }

                        tData = null;

                        //add to todo list
                        toAdd.Add(ele.getInternalId());
                    }

                }

                SvgElement child = ele.getChild();
                while (child != null)
                {
                    AddFromSvg2(child);
                    child = child.getNext();
                }
                ele = ele.getNext();
            }
            
        }




        protected void btnCalc_Click(object sender, EventArgs e)
        {
            // clear error messages
            dialog.InnerHtml = "";

            
            // FORM ITEMS ================================================================
            //file info:
            fileName.InnerHtml = HttpUtility.HtmlEncode(Context.Request.Form["cFileName"]);
            string fPath = Context.Server.MapPath(@"imgLibrary");
            string fullPath = fPath + @"\" + Context.Request.Form["cFileName"];
            string fName = Context.Request.Form["cFileName"];
            
            // user selected options:
            int materialId = Convert.ToInt32(svgMaterial.SelectedValue);
            int quantity = Convert.ToInt32(svgQuantity.Text);
            int mmId = Convert.ToInt32(svgMm.SelectedValue);

            // END FORM ==================================================================


            //get the template size id from the uloaded file
            templateSize myTemp = new templateSize();
            int templateSizeId = myTemp.detectTemplate(fullPath);

            if (templateSizeId == 0)
            {
                // is not a valid template - show error
                errors errType = new errors();
                errType.errTemplate();
                openError(errType.currentError + errType.helpUrl);
                errType = null;
                return;

            }
                        
            //show template size on screen
            lblSvgTemplateSize.Text = "Template size " + myTemp.txtTemplateSize;            
            myTemp = null;


            //load the svg doc into the svg library
            SvgDoc myDoc = new SvgDoc();
            myDoc.LoadFromFile(fullPath);         


            SVGLib.SvgElement elYourDesigns = myDoc.GetSvgElement("Your_Designs");
            if (elYourDesigns.getChild() != null)
            {
                AddFromSvg2(elYourDesigns);
            }
            else
            {
                //check inkscape added group labled "Your Designs"
                // get the id by searching for the label then us the ID with getSvgElement
                inkscapeGroupFix gf = new inkscapeGroupFix();
                string groupId = gf.getgroupId("Your Designs", fullPath, "Your Designs");
                if (groupId == "")
                {
                    //throw error, no design detected
                    errors errType = new errors();
                    errType.errDesign();
                    openError(errType.currentError + errType.helpUrl);
                    errType = null;
                    return;
                }
                else
                {
                    elYourDesigns = myDoc.GetSvgElement(groupId);
                    AddFromSvg2(elYourDesigns);
                }

                gf = null;
            }

            myDoc = null;
            
            List<shapePrice> priceArray = allShapes;
            zapQuote newQuote = new zapQuote(materialId, templateSizeId, mmId);

            //check if error returned from AddFromSvg
            if (isValidSvgDesign == false)
            {
                //show error and halt progress
                openError(svgError);
                return;
            }

            
            foreach (shapePrice i in priceArray)
            {  

                newQuote.appendShapeData(i.length, i.area, i.zapType, i.height, i.xpos, i.ypos);
                // raster line area from objects
                rasterExtraArea += i.rasterArea;
            }

            
            //calculate extra raster area from coordinates
            if (rasterPoints.Count > 0)
            {
                //find 4 outer points and calculate area as a rectangle

                double lX = rasterPoints.Min(c => c.X);
                double rX = rasterPoints.Max(c => c.X);
                double tY = rasterPoints.Min(c => c.Y);
                double bY = rasterPoints.Max(c => c.Y);

                rasterExtraArea = Common.rasterAreaFromPoints(lX, rX, tY, bY);
                extraHeight = Common.rasterHeight(tY, bY);
            }




            newQuote.calcTotalTime(materialId, templateSizeId, mmId, rasterExtraArea, extraHeight);

            

            materialPrice mPrice = new materialPrice();

            mPrice.getMaterialPrices(materialId, templateSizeId, mmId);


            //cut lines price
            decimal cutPrice = Convert.ToDecimal(newQuote.cutTime * mPrice.costCutSec);
            // engrave lines
            decimal engravePrice = Convert.ToDecimal(newQuote.engraveTime * mPrice.costVectorSec);
            // raster engrave
            //decimal rasterPrice = Convert.ToDecimal((newQuote.totalAreaFill / 100) * mPrice.costRasterPerSqCm);

            decimal rasterPrice = Convert.ToDecimal(newQuote.fillTime  * mPrice.costRasterPerSec);

            decimal totalLaserPrice = cutPrice + engravePrice + rasterPrice;

            decimal materialCost = Convert.ToDecimal(mPrice.pricePerUnit);

            decimal unitPrice = totalLaserPrice + materialCost;

            decimal totalPrice = unitPrice * quantity;


            //populate labels / session values

            StringBuilder quoteInfo = new StringBuilder();

            quoteInfo.Append("<ul>");
                quoteInfo.Append("<li><b>File Cost</b>");
                quoteInfo.Append("<div class='formInfo'>- " + fName + " = <span class='formBlue'>" + string.Format(CultureInfo.CreateSpecificCulture("en-GB"), "{0:C}", totalLaserPrice) + "</span></div>");
                    quoteInfo.Append("<div class='formInfo'>- Quantity = <span class='formBlue'>x" + quantity + "</span></div>");
                    quoteInfo.Append("<div class='formBlue'>Total File Cost = " + string.Format(CultureInfo.CreateSpecificCulture("en-GB"), "{0:C}", totalLaserPrice * quantity) + "</div>");
                quoteInfo.Append("</li>");
                quoteInfo.Append("<li><b>Material Cost</b>");
                    quoteInfo.Append("<div class='formInfo'>- " + svgMaterial.SelectedItem.Text + "</div>");
                    quoteInfo.Append("<div class='formInfo'>- " + svgMm.SelectedItem.Text + "</div>");
                    quoteInfo.Append("<div class='formInfo'>- " + materialColour.SelectedItem.Text + "</div>");
                    quoteInfo.Append("<div class='formInfo'>- = <span class='formBlue'>" + string.Format(CultureInfo.CreateSpecificCulture("en-GB"), "{0:C}", materialCost) + "</span></div>");
                    quoteInfo.Append("<div class='formInfo'>- Quantity = <span class='formBlue'>x" + quantity + "</span></div>");
                    quoteInfo.Append("<div class='formInfo'>- Total material cost= <span class='formBlue'>" + string.Format(CultureInfo.CreateSpecificCulture("en-GB"), "{0:C}", materialCost*quantity) + "</span></div>");
                quoteInfo.Append("</li>");
                quoteInfo.Append("<li><b>Total Cost</b> <span class='formBlue'>" + string.Format(CultureInfo.CreateSpecificCulture("en-GB"), "{0:C}", totalPrice) + "</span>");
                quoteInfo.Append("</li>");
            quoteInfo.Append("</ul>");

            divQuote.InnerHtml = quoteInfo.ToString();

            quoteInfo = null;



            StringBuilder debugInfo = new StringBuilder();


            debugInfo.Append("<p>Cut price:" + cutPrice + "</p>");
            debugInfo.Append("<p>Cut time:" + newQuote.cutTime + "</p>");
            debugInfo.Append("<p>Cut length mm:" + newQuote.totalCutLength + "</p>");
            debugInfo.Append("<p>Cut cost per sec:" + mPrice.costCutSec + "</p>");
            debugInfo.Append("<p>Number of cuts:" + newQuote.cutCount + "</p>");
            debugInfo.Append("<p>Cut area:" + newQuote.totalCutArea  + "</p>");

            debugInfo.Append("<p>Vector engrave price:" + engravePrice + "</p>");
            debugInfo.Append("<p>Vector engrave time:" + newQuote.engraveTime + "</p>");
            debugInfo.Append("<p>Vector engrave cost per sec:" + mPrice.costVectorSec + "</p>");

            debugInfo.Append("<p>Vector engrave light length:" + newQuote.totalLightEngrave+ "</p>");
            debugInfo.Append("<p>Vector engrave med length:" + newQuote.totalMedEngrave + "</p>");
            debugInfo.Append("<p>Vector engrave heavy length:" + newQuote.totalHeavyEngrave+ "</p>");


            debugInfo.Append("<p>Raster price:" + rasterPrice  + "</p>");
            debugInfo.Append("<p>Raster time:" + newQuote.fillTime + "</p>");
            debugInfo.Append("<p>Raster cost per sec:" + mPrice.costRasterPerSec + "</p>");
            debugInfo.Append("<p>Raster area:" + newQuote.rasterArea + "</p>");


          


            litErr.Text = debugInfo.ToString();

            debugInfo = null;
        }



      

        protected void svgMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (svgMaterial.SelectedValue != string.Empty)
            {
                try
                {
                    materialMMid.SelectParameters["materialTypeId"].DefaultValue = Convert.ToString(svgMaterial.SelectedValue);
                    materialMMid.DataBind();
                }

                catch (Exception ex) { }
            }

        }



        protected void openError(string errHtml)
        {

            dialog.InnerHtml = errHtml;

           // runjQueryCode("dlg = $('#zappquote_dialog').dialog({autoOpen: false, hide: 'clip', title: 'Error in your design', closeText: 'hide' }); $('#zappquote_dialog').dialog('open')");


        }



        private string getjQueryCode(string jsCodetoRun)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("$(document).ready(function() {");
            sb.AppendLine(jsCodetoRun);
            sb.AppendLine(" });");

            return sb.ToString();
        }

        private void runjQueryCode(string jsCodetoRun)
        {            
                ScriptManager.RegisterClientScriptBlock(this,
                                                        typeof(Page),
                                                        Guid.NewGuid().ToString(),
                                                        getjQueryCode(jsCodetoRun),
                                                        true);            
        }




    }
}