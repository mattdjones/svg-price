using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace priceSvg
{
    public class zapCompliance
    {

        public bool validStroke = true;
        public bool validFill = true;
        public bool validElement = true;
        public int zapType;
        public int zapErrorType;
        public bool lineToThick = false;
        

        public zapCompliance()
        {
            // maybe use to check element is valid?
        }


        public void checkStrokeFill(string sColor, string fColor, string sWidth, bool hasFillColor)
        {
            //check that the fill and stroke conform to Zaps spec

            // if a shape has no stroke or fill specified it will be filled black

            if (hasFillColor)
            {
                //check if colour is white and ignore
                // also check for black
                if (checkIgnoreColour(fColor))
                {
                    hasFillColor = false;
                }
            }
            else
            {
                //check if stroke is white and ignore too
                if (checkIgnoreColour(sColor))
                {
                    validElement = true;
                    validFill = false;
                    zapType = 0;
                    return;
                }
            }

            
            if (hasFillColor)
            {
                //check fill color

                int fillType = getFillType(fColor);
                if (fillType == 0)
                {
                    validFill = false;
                    zapErrorType = zapConstants.errUnknownFill;
                    validElement = false;
                    zapType = 0;
                    return;
                }
                else 
                {
                    validElement = true;
                    validFill = true;
                    zapType = fillType;
                    return;
                }
                                    
            }
            else
            {
                
                //check cut and engrave lines
                if (sColor != string.Empty)
                {

                    int lineType = getLineType(sColor);
                    if (lineType == 0)
                    {

                        //check if it's a raster line 
                        if(rasterLineWidth(sWidth))
                        {
                            if (sColor == zapConstants.rasterLightEngraveColor || sColor == zapConstants.rasterMedEngraveColor || sColor == zapConstants.rasterHeavyEngraveColor)
                            {
                                zapType = zapConstants.rasterLine;
                                validElement = true;
                                validStroke = true;
                                return;
                            }
                            else
                            {
                                //unknown line colour
                                zapErrorType = zapConstants.errColourMisMatch;
                                validElement = false;
                                validStroke = false;
                                return;
                            }
                        }
                        else
                        {
                            //unknown line colour
                            zapErrorType = zapConstants.errColourMisMatch;
                            validElement = false;
                            validStroke = false;
                        }           
                    }
                    else
                    {
                        lineToThick = getLineWidth(sWidth);

                        switch (lineType)
                        {
                            case zapConstants.vectorCut:
                                zapType = zapConstants.vectorCut;
                                if (lineToThick)
                                    zapErrorType = zapConstants.errCutToThick;                                
                                break;

                            case zapConstants.vectorLightEngrave:
                                zapType = zapConstants.vectorLightEngrave;
                                if (lineToThick)
                                    zapErrorType = zapConstants.errEngraveToThickBlue;
                                break;

                            case zapConstants.vectorMedEngrave:
                                zapType = zapConstants.vectorMedEngrave;
                                if (lineToThick)
                                    zapErrorType = zapConstants.errEngraveToThickGreen;
                                break;

                            case zapConstants.vectorHeavyEngrave:
                                zapType = zapConstants.vectorHeavyEngrave;
                                if (lineToThick)
                                    zapErrorType = zapConstants.errEngraveToThickMagenta;
                                break;

                            default:
                                zapType = 0;
                                zapErrorType = zapConstants.errMiscStrokeErr;
                                break;
                        }

                        if (lineToThick)
                        {
                            validElement = false;
                            validStroke = false;
                            return;
                        }
                        else
                        {
                            validElement = true;
                            validStroke = true;
                            return;
                        }

                    }
                    
                }
                else
                {
                    zapErrorType = zapConstants.errMiscStrokeErr;

                }
            }



        }




        protected int getLineType(string sColor)
        {
            int lineType = 0;

            switch (sColor)
            {
                case zapConstants.vectorCutColor:
                    lineType = zapConstants.vectorCut;                    
                    break;

                case zapConstants.vectorLightEngraveColor:
                    lineType = zapConstants.vectorLightEngrave;                    
                    break;

                case zapConstants.vectorMedEngraveColor:
                    lineType = zapConstants.vectorMedEngrave;                    
                    break;

                case zapConstants.vectorHeavyEngraveColor:
                    lineType = zapConstants.vectorHeavyEngrave;                    
                    break;

                default:
                    lineType = 0;
                    break;
            }

            return lineType;
        }

        protected bool getLineWidth(string sWidth)
        {
            double lineWidth = Convert.ToDouble(sWidth);

            return lineWidth > zapConstants.maxLineWidth;
            
        }

        protected bool rasterLineWidth(string sWidth)
        {
            double lineWidth = Convert.ToDouble(sWidth);

            return lineWidth >= zapConstants.rasterLineMinWidth;

        }

        protected int getFillType(string fColor)
        {
            int fillType = 0;
            switch (fColor)
            {
                case zapConstants.rasterLightEngraveColor:
                    fillType = zapConstants.rasterLightEngrave;
                    break;
                case zapConstants.rasterMedEngraveColor:
                    fillType = zapConstants.rasterMedEngrave;
                    break;
                case zapConstants.rasterHeavyEngraveColor:
                    fillType = zapConstants.rasterHeavyEngrave;
                    break;
                case zapConstants.rasterIgnoreColour:
                    fillType = 0;
                    break;
                default:
                    fillType = 0;
                    break;
            }

            return fillType;

        }



        private bool checkIgnoreColour(string cColour)
        {
            if (cColour == zapConstants.rasterIgnoreColour)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

    }
    

}