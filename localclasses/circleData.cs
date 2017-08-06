using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SVGLib;
using System.Drawing;

namespace priceSvg
{
    public class circleData
    {

        public double radius;
        public string strokeColor;
        public string strokeWidth;
        public double xpos;
        public double ypos;
        public string fillColor;
        public string Id;
        public string style;

        public circleData(SvgCircle svg)
        {

            style = svg.Style;
            if (style == "")
            {
                Color cColor = svg.Stroke;
                strokeColor = Convert.ToString(cColor.R) + Convert.ToString(cColor.G) + Convert.ToString(cColor.B);
                strokeWidth = svg.StrokeWidth;
                Color fColor = svg.Fill;
                fillColor = Convert.ToString(fColor.R) + Convert.ToString(fColor.G) + Convert.ToString(fColor.B);

            }
            else
            {
                //get from style attribute
                extractStyle rStyle = new extractStyle();
                rStyle.getStyle(style);
                Color sCol = ColorTranslator.FromHtml(rStyle.strokeColour);
                Color fCol = ColorTranslator.FromHtml(rStyle.fillColour);
                strokeColor = Convert.ToString(sCol.R) + Convert.ToString(sCol.G) + Convert.ToString(sCol.B);
                strokeWidth = rStyle.strokeWidth;
                fillColor = Convert.ToString(fCol.R) + Convert.ToString(fCol.G) + Convert.ToString(fCol.B);
            }

            radius = Convert.ToDouble(svg.R);            
            xpos = Convert.ToDouble(svg.CX);
            ypos = Convert.ToDouble(svg.CY);            
            Id = svg.Id;
            
        }

    }
}