using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priceSvg
{
    public class shapePrice
    {
        public int zapType;
        public double length;
        public double area;
        public double xpos;
        public double ypos;
        public double height;
        public string otherInfo;
        public double rasterArea;

        public shapePrice(int zt, double le, double ar, double x, double y, string info, double h, double ra)
        {
            zapType = zt;
            length = le;
            area = ar;
            xpos = x;
            ypos = y;
            otherInfo = info;
            height = h;
            rasterArea = ra;
        }

    }
}