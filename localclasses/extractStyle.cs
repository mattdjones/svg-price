using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace priceSvg
{
    public class extractStyle
    {
        public string strokeColour;
        public string fillColour;
        public string strokeWidth;

        public void getStyle(string styles)
        {
            //required attributes
            string preStroke = "stroke";
            string preFill = "fill";
            string preWidth = "stroke-width";

            //extract with regex

            Regex regex = new Regex(@"(?<type>[\w\d-]*):\s*(?<value>[^;]+);");

            var results = regex.Matches(styles);
            
            foreach(Match match in results)
            {
                if(match.Groups["type"].Value==preStroke)
                {
                    strokeColour = match.Groups["value"].Value;
                    if (strokeColour == "none")
                    {
                        strokeColour = "#ffffff";
                    }
                }
                if (match.Groups["type"].Value == preFill)
                {
                    fillColour = match.Groups["value"].Value;
                    if (fillColour == "none")
                    {
                        fillColour = "#ffffff";
                    }
                }
                if (match.Groups["type"].Value == preWidth)
                {
                    strokeWidth = match.Groups["value"].Value;
                }
            }


        }


    }
}