using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SVGLib;
using System.Xml.Linq;

namespace priceSvg
{
    public class errors
    {

        public bool hasError;
        public string currentError;
        public string helpUrl;

                        

        public void errIsSvgFile(string pathToSvg)
        {
            SvgDoc checkDoc = new SvgDoc();
            if (checkDoc.LoadFromFile(pathToSvg))
            {
                // can proceed is valid svg Document
                hasError = false;
                currentError = @"<p><strong>Ooops! - Incorrect File Format</strong></p><p>I'm sorry, but we do not accept this file format.</p><p>Please resave your work as an .svg file and resubmit your file.</p>";
                helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> to find out how to save your files in the correct format.</p>";
            }
            else
            {
                hasError = false;
                currentError = "";
            }
            checkDoc = null;
        }


        /// <summary>
        /// Checks that the customer files is not bigger than the 10 mb limit.
        /// </summary>
        /// <returns>hasError boolean and currentError string message.</returns>        
        public void errFileSize(double fileBytes)
        {

            double mbSize = (fileBytes / 1024f) / 1024f;
            if (mbSize >= 1)
            {
                hasError = true;
                currentError = @"<p><strong>Ooops! - File Size Too Big</strong></p><p>Your file has exceeded our 10mb limit. Please break your design down and resubmit your file.</p>";
                helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> for tips on reducing your file sizes.</p>";
            }
            else
            {
                hasError = false;
                currentError = "";
                helpUrl = "";
            }  
        }


        /// <summary>
        /// Displays error when there is nothing in the Your Designs group.
        /// </summary>
        /// <returns>hasError boolean and currentError string message.</returns>        
        public void errDesign()
        {            
                hasError = true;
                currentError = @"<p><strong>Ooops! - No design detected</strong></p><p>No design was detected in the file you uploaded.</p>";
                helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> for tips on using the templates.</p>";            
        }



        /// <summary>
        /// Checks that the customer files is using one of the downloadable templates.
        /// </summary>
        /// <returns>hasError boolean and currentError string message & string helpUrl.</returns>        
        public void errTemplate()
        {

            hasError = true;
            currentError = @"<p><strong>Ooops! - No template error</strong></p><p>NThe file you uploaded is not recognised as a valid template.</p>";
            helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> for tips on using the templates.</p>"; 

        }


        /// <summary>
        /// Checks if the customer files has images in the design.
        /// </summary>
        /// <returns>hasError boolean and currentError string message & string helpUrl.</returns>        
        public void errBitmap()
        {
            //SvgDoc clientDoc
            //SvgElement myEl = clientDoc.GetSvgElement("Your_Designs");
            //XElement tag = XElement.Parse(myEl.GetTagXml());
            //IEnumerable<XElement> svgElements = tag.Descendants();

            //foreach (XElement svgEle in svgElements)
            //{
            //    //check for image tag
            //    if (svgEle.Name == "image")
            //    {
                    hasError = true;
                    currentError = @"<p><strong>Ooops! - Bitmap Blunder!</strong></p><p>We have detected a bitmap image in your file. Please convert all bitmaps into vectors and resubmit your file.</p>";
                    helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> to find out how to convert your bitmap images into vector images.</p>";
            //    }
            //    else
            //    {
            //        hasError = false;
            //        currentError = "";
            //        helpUrl = "";
            //    }
            //}
            //myEl = null;
            //tag = null;

        }


        public void getElementError(int errType)
        {
            hasError = true;

            switch (errType)
            {
                case zapConstants.errCutToThick:
                    currentError = @"<p><strong>Ooops! - Cutline Issue!</strong></p><p>One or more of your cutlines are too thick. Please check that all cutlines are 0.01mm thick and resubmit your file.</p>";
                    helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> for tips on correcting this issue.</p>";
                    break;
                case zapConstants.errCutContainsFill:
                    currentError = @"<p><strong>Ooops! - Cutline Issue</strong></p><p>One or more of your cutlines contain a fill!</p>";
                    helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> for tips on correcting this issue.</p>";
                    break;
                case zapConstants.errEngraveToThickBlue:
                    currentError = @"<p><strong>Ooops! - Blue Engraving Issue</strong></p><p>One or more of your blue vector engraving lines are too thick! Please check that all vector lines are 0.01mm thick and resubmit your file.</p>";
                    helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> for tips on correcting this issue.</p>";
                    break;
                case zapConstants.errEngraveToThickGreen:
                    currentError = @"<p><strong>Ooops! - Green Engraving Issue</strong></p><p>One or more of your green vector engraving lines are too thick! Please check that all vector lines are 0.01mm thick and resubmit your file.</p>";
                    helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> for tips on correcting this issue.</p>";
                    break;
                case zapConstants.errEngraveToThickMagenta:
                    currentError = @"<p><strong>Ooops! - Magenta Engraving Issue</strong></p><p>One or more of your magenta vector engraving lines are too thick! Please check that all vector lines are 0.01mm thick and resubmit your file.</p>";
                    helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> for tips on reducing your file sizes.</p>";
                    break;
                case zapConstants.errEngraveFillBlue:
                    currentError = @"<p><strong>Ooops! - Blue Engraving Issue</strong></p><p>One or more of your blue vector engraving lines contain a fill.</p>";
                    helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> for tips on correcting this issue.</p>";
                    break;
                case zapConstants.errEngraveFillGreen:
                    currentError = @"<p><strong>Ooops! - Green Engraving Issue</strong></p><p>One or more of your green vector engraving lines contain a fill.</p>";
                    helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> for tips on correcting this issue.</p>";
                    break;
                case zapConstants.errEngraveFillMagenta:
                    currentError = @"<p><strong>Ooops! - Magenta Engraving Issue</strong></p><p>One or more of your magenta vector engraving lines contain a fill.</p>";
                    helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> for tips on correcting this issue.</p>";
                    break;
                case zapConstants.errColourMisMatch:
                    currentError = @"<p><strong>Ooops! - Colour Mismatch</strong></p><p>You appear to have used a colour which we do not recognise.<br />Please check your colours and resubmit your file.</p>";
                    helpUrl = @"<p>Click <a href=""\help\?helipid=""here</a> to download our approved colour swatches.</p>";
                    break;
                case zapConstants.errMiscStrokeErr:
                    currentError = @"<p><strong>Ooops! - Miscellaneous Error</strong></p><p>I'm sorry but there is problem with your file which we are unable to identify!</p><p>Please go through your document and check that all vector and raster details are correct and in accordance with our submission checklist and then resubmit your file.</p><p>If this problem persists please send your file to problems@zapcreatives.com and we will contact you asap!</p>";
                    helpUrl = @"";
                    break;
                case zapConstants.errUnknownFill:
                    currentError = @"<p><strong>Ooops! - Fill Colour Error</strong></p><p>I'm sorry but there is problem with your file, you are using an invalid fill colour!</p><p>Please go through your document and check that all vector and raster details are correct and in accordance with our submission checklist and then resubmit your file.</p><p>If this problem persists please send your file to problems@zapcreatives.com and we will contact you asap!</p>";
                    helpUrl = @"";
                    break;
                case zapConstants.errContainsText:
                    currentError = @"<p><strong>Ooops! - Text Issue</strong></p><p>We have detected text within your document.</p><p>Please convert all text to outlines (paths) and resubmit your file.</p>";
                    helpUrl = @"";
                    break;

            }


        }

    }
}