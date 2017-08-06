using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SVGLib;
using System.Drawing;

namespace priceSvg
{
    public class lineData
    {                       
        public string strokeColor;
        public string strokeWidth;
        public double xpos1;
        public double xpos2;
        public double ypos1;
        public double ypos2;
        public string fillColor;
        public string Id;
        public string style;

        public lineData(SvgLine svg)
        {
            
            Color cColor = svg.Stroke;
            strokeColor = Convert.ToString(cColor.R) + Convert.ToString(cColor.G) + Convert.ToString(cColor.B);
            strokeWidth = svg.StrokeWidth;
            xpos1 = Convert.ToDouble(svg.X1);
            xpos2 = Convert.ToDouble(svg.X2);
            ypos1 = Convert.ToDouble(svg.Y1);
            ypos2 = Convert.ToDouble(svg.Y2);
            Color fColor = svg.Fill;
            fillColor = Convert.ToString(fColor.R) + Convert.ToString(fColor.G) + Convert.ToString(fColor.B);
            Id = svg.Id;
            style = svg.Style;            
            
        }
    }
}