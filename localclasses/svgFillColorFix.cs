using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;


namespace priceSvg
{
    public class svgFillColorFix
    {
        public static bool checkFill(string cXml)
        {
            try
            {
                XElement tag = XElement.Parse(cXml);


                if (tag.Attribute("fill") == null)
                {
                    //check style element
                    if (tag.Attribute("style") == null)
                    {
                        return false;
                    }
                    else
                    {
                        string style = tag.Attribute("style").Value.ToString();
                        //get from style attribute
                        extractStyle rStyle = new extractStyle();
                        rStyle.getStyle(style);

                        if (rStyle.fillColour != string.Empty)
                        {
                            rStyle = null;
                            tag = null;
                            return true;
                        }
                        else
                        {
                            rStyle = null;
                            tag = null;
                            return false;
                        } 
                    }
                    
                }
                else
                {
                    string fillValue = tag.Attribute("fill").Value.ToString();

                    if (fillValue == "none")
                    {
                        tag = null;
                        return false;
                    }
                    else
                    {
                        tag = null;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

                return false;

            }

        }


    }
}