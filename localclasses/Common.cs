using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace priceSvg
{
    public class Common
    {

        public static double rasterAreaFromPoints(double lx, double rx, double ty, double by)
        {

            Point topLeft = new Point(lx, ty);
            Point topRight = new Point(rx, ty);
            Point bottomLeft = new Point(lx, by);
            Point bottomRight = new Point(rx, by);


            double xLine = myShapes.lineLength(lx, rx, ty, ty);
            double yLine = myShapes.lineLength(lx, rx, ty, by);


            return myShapes.convertPixelToMm(xLine * yLine);

            //StringBuilder sb = new StringBuilder();
            //sb.Append(topLeft.X + "," + topLeft.Y + " " + topRight.X + "," + topRight.Y + " " + bottomRight.X + "," + bottomRight.Y + " " + bottomLeft.X + "," + bottomLeft.Y);
            //string polyPoints = sb.ToString();
            //sb = null;

            //myShapes objShape = new myShapes();
            //double rArea = objShape.polygonArea(polyPoints);
            //objShape = null;

            //return myShapes.convertPixelToMm(rArea);

        }

        public static double rasterHeight(double ty, double by)
        {
            return myShapes.convertPixelToMm( Math.Abs(ty - by));
        }


        public static string cleanString(string cInput)
        {
            string s = cInput;

            //char tab = '\u0009';
            //char newline = '\u000A';

            s = s.Replace("\n", String.Empty);
            s = s.Replace("\r", String.Empty);
            s = s.Replace("\t", String.Empty);

            return s;

        }


        public static string convertPolyForSql(string cInput, bool isPolygon)
        {
            string s = cInput;
            //replace comma with new character
            s = s.Replace(",", "@");
            //replace spaces with commas
            s = s.Replace(" ", ",");
            //replace @ with spaces
            s = s.Replace("@", " ");

            if(isPolygon) {
                //get first points to append to end
                string[] points;
                points = s.Split(',');
                s += "," + points[0];
            }
            return s;
        }

    }
}