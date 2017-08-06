using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;
using SharpVectors.Dom;
using SharpVectors.Dom.Svg;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Data.SqlClient;
using System.Configuration;




namespace priceSvg
{
    public class myShapes
    {


        public static double circleArea(double radius)
        {
            double t = radius * radius * System.Math.PI;
            return convertPixelToMm(t);
        }


        public static double circleCircumference(double radius)
        {
            double t = radius * 2 * System.Math.PI;
            return convertPixelToMm(t);
        }


        public static double circleHeight(double radius)
        {
            double t = radius * 2;
            return convertPixelToMm(t);
        }

        public static double lineLength(double x1, double x2, double y1, double y2)
        {
            //sql built in method

            string linePoints = Convert.ToString(x1) + " " + Convert.ToString(y1) + ", " + Convert.ToString(x2) + " " + Convert.ToString(y2);

            return convertUnitToMm(sqlLineLength(linePoints));

            //use pythagoras to return length of a line from coordinates
            //pythagorean theorem c^2 = a^2 + b^2
            //thus c = square root(a^2 + b^2)
            
            //old below here
            //double a;
            //if(x1 == x2){
            //    a = x1;
            //}
            //else{
            //    a = (x1 - x2);
            //}
            
            //double b = (y1 - y2);
            //return convertUnitToMm(Math.Sqrt(a * a + b * b));
            
        }

        public static double sqlpolyLineLength(string polyPoints)
        {
            double returnLength = 0.0;

            string[] points;
            string cleanPoints, sqlPoints;

            cleanPoints = Common.cleanString(polyPoints);
            cleanPoints = cleanPoints.Replace("  ", " ");
            points = cleanPoints.Split(' ');

            sqlPoints = Common.convertPolyForSql(cleanPoints, false);

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["zapCartDb"].ConnectionString);
            myConn.Open();
            string polySql = @"DECLARE @g2 geometry = 'LINESTRING(" + sqlPoints + ")'; select @g2.MakeValid().STLength();";
            SqlCommand myCmd = new SqlCommand(polySql);
            myCmd.Connection = myConn;
            SqlDataReader myReader = null;
            myReader = myCmd.ExecuteReader();

            while (myReader.Read())
            {
                returnLength = (double)myReader[0];
            }
            myReader = null;
            myConn.Close();
            myCmd = null;
            myConn = null;

            return convertPixelToMm(returnLength);
        }

        public static double sqlLineLength(string polyPoints)
        {
            double returnLength = 0.0;

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["zapCartDb"].ConnectionString);
            myConn.Open();
            string polySql = @"DECLARE @g2 geometry = 'LINESTRING(" + polyPoints + ")'; select @g2.MakeValid().STLength();";
            SqlCommand myCmd = new SqlCommand(polySql);
            myCmd.Connection = myConn;
            SqlDataReader myReader = null;
            myReader = myCmd.ExecuteReader();

            while (myReader.Read())
            {
                returnLength = (double)myReader[0];
            }
            myReader = null;
            myConn.Close();
            myCmd = null;
            myConn = null;

            return returnLength;
        }

        public static double distance(double x1, double x2, double y1, double y2)
        {
            //double dx = x1 - x2;         //horizontal difference 
            double dx = Math.Abs(x1 - x2);         //horizontal difference 
            //double dy = y1 - y2;         //vertical difference 
            double dy = Math.Abs(y1 - y2);         //vertical difference 
            double dist = Math.Sqrt(dx * dx + dy * dy); //distance using Pythagoras theorem
            return convertUnitToMm(dist);
        }

        public static double ellipseArea(double rx, double ry)
        {
            //double check rx and ry is already 

            return convertUnitToMm(Math.PI * rx * ry);
            
        }

        public static double ellipsePerimeter(double rx, double ry)
        {
            //PI * SquareRoot of 2 * ((1/2 long axis)squared + (1/2 short axis)squared)
            double lAxis, sAxis, lR, sR;

            if (rx > ry)
            {
                lAxis = rx * 2;
                sAxis = ry * 2;
                lR = rx;
                sR=ry;
            }
            else
            {
                lAxis = ry * 2;
                sAxis = rx * 2;
                lR = rx;
                sR=ry;
            }            
            //use / 2 if svg
           // return ((Math.Sqrt(.5 * ((lR * lR) + (sR * sR)))) * (Math.PI * 2)) / 2;

            return convertUnitToMm((Math.Sqrt(.5 * ((lR * lR) + (sR * sR)))) * (Math.PI * 2));

        }

        public static double rectangleArea(double rH, double rW)
        {
            //TODO convert to milimeters from whichever unit is used
            return convertUnitToMm(rH * rW);
        }

        public static double rectanglePerimeter(double rH, double rW)
        {
            return convertUnitToMm(2 * (rH + rW));
        }


        public static double polygonLength(string svgpoints)
        {
                        
            string[] points;
            string cleanPoints, sqlPoints;

            cleanPoints = Common.cleanString(svgpoints);
            cleanPoints = cleanPoints.Replace("  ", " ");            
            points = cleanPoints.Split(' ');

            sqlPoints = Common.convertPolyForSql(cleanPoints, true);

            return convertPixelToMm(sqlPolygonLength(sqlPoints));

            //int sides = 0;
            //double plineLength = 0;

            //// work out total length of lines in the polygon
            //int lastPoint = points.Count();
            //sides = lastPoint - 1;

            //for (int i = 0; i <= sides; i++)
            //{
            //    string[] xycoords = points[i].Split(',');

            //    if (i != sides)
            //    {
            //        string[] xycoords2 = points[i + 1].Split(',');
            //        plineLength += lineLength(Convert.ToDouble(xycoords[0]), Convert.ToDouble(xycoords[0]), Convert.ToDouble(xycoords2[0]), Convert.ToDouble(xycoords2[1]));
            //    }
            //    else
            //    {
            //        string[] xycoords1 = points[0].Split(',');
            //        plineLength += lineLength(Convert.ToDouble(xycoords[0]), Convert.ToDouble(xycoords[0]), Convert.ToDouble(xycoords1[0]), Convert.ToDouble(xycoords1[1]));
            //    }

            //}

            //return convertUnitToMm(plineLength);
        }

        public static double sqlPolygonLength(string polyPoints)
        {
            double returnLength = 0.0;

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["zapCartDb"].ConnectionString);
            myConn.Open();
            string polySql = @"DECLARE @g2 geometry = 'POLYGON((" + polyPoints + "))'; select @g2.MakeValid().STLength();";            
            SqlCommand myCmd = new SqlCommand(polySql);
            myCmd.Connection = myConn;                       
            SqlDataReader myReader = null;
            myReader = myCmd.ExecuteReader();

            while (myReader.Read())
            {
                returnLength = (double)myReader[0];
            }
            myReader = null;
            myConn.Close();
            myCmd = null;
            myConn = null;

            return returnLength;
        }


        public static double polygonArea(string svgpoints)
        {

            string cleanPoints, sqlPoints;
            cleanPoints = Common.cleanString(svgpoints);
            cleanPoints = cleanPoints.Replace("  ", " ");

            sqlPoints = Common.convertPolyForSql(cleanPoints, true);

            return convertPixelToMm(sqlPolygonArea(sqlPoints)); 

            //string[] points = cleanPoints.Split(' ');
            //int sides = 0;
            //List<double> xp = new List<double>();
            //List<double> yp = new List<double>();            
            //int counter = 0;

            ////put the vertices in an array for x and y
            //foreach (string point in points)
            //{
            //    string[] xycoords = point.Split(',');
            //    xp.Add(Convert.ToDouble(xycoords[0]));
            //    yp.Add(Convert.ToDouble(xycoords[1]));
            //    counter +=1;
            //    sides += 1;
            //}
           
            ////return convertUnitToMm(AreaPolygon(xp, yp, sides - 1));            

            //return convertPixelToMm(AreaPolygon(xp, yp, sides - 1));            
        }

        public static double sqlPolygonArea(string polyPoints)
        {
            double returnLength = 0.0;
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["zapCartDb"].ConnectionString);
            myConn.Open();
            string polySql = @"DECLARE @g2 geometry = 'POLYGON((" + polyPoints + "))'; select @g2.MakeValid().STArea();";
            SqlCommand myCmd = new SqlCommand(polySql);
            myCmd.Connection = myConn;  
            SqlDataReader myReader = null;
            myReader = myCmd.ExecuteReader();

            while (myReader.Read())
            {
                returnLength = (double)myReader[0];
            }

            myReader = null;
            myConn.Close();
            myCmd = null;
            myConn = null;

            return returnLength;
        }

        private static double AreaPolygon(List<double> x, List<double> y, int noOfPoints)
        {
            double area = 0;
            for (int i = 0; i <= noOfPoints; i++)
            {
                if (i == noOfPoints)
                {
                    area = area + ((x[i] * y[0]) - (x[0] * y[i]));
                }
                else
                {
                    area = area + ((x[i] * y[i + 1]) - (x[i + 1] * y[i]));
                }
            }
           // area = area * 0.5;
           // return area;

            //return Math.Abs(area * 0.5);

            if (area < 0)
            {
                area *= -1;
            }
            return area/2;

        }


     

        // hanlde Paths :

        public static double getPathLength(string myPath) 
        {
            // just use sharpVector segmentList 
             SvgPathSegList mylist = new SvgPathSegList(myPath, true);            
             Double pathLength = mylist.GetTotalLength();            
             mylist = null;
            
             //return convertUnitToMm(pathLength);            
             return convertPixelToMm(pathLength);   
        }


        public static double getPathAreaInk(string myPath)
        {

            //string newPath = HttpContext.Current.Server.MapPath("isPath.py");

           // var ipy = Python.CreateRuntime();
           // dynamic pathCall = ipy.ExecuteFile(zapConstants.pythonMethodPath);

            try {
                double pathArea = Convert.ToDouble(zapConstants.pathCall.Simple(myPath));
                //ipy = null;
                //pathCall = null;
                return pathArea;
            }
            catch (Exception e)
            {
                //ipy = null;
                //pathCall = null;
                return 0;
            }
            
        }


        /// <summary>
        /// Not using returns negative value if points in wrong order
        /// </summary>
        public double getPathArea(string myPath)
        {
            // just use sharpVector segmentList 
            SvgPathSegList mylist = new SvgPathSegList(myPath, true);
            StringBuilder sb = new StringBuilder();
            double fX = 0.0;
            double fY = 0.0;

            int segCount = 0;
            foreach (SvgPointF seg in mylist.Points)
            {

                sb.Append(Convert.ToString(seg.ValueX) + "," + Convert.ToString(seg.ValueY));
                segCount += 1;
                
                if (segCount < mylist.NumberOfItems)
                {                    
                        sb.Append(" ");                    
                }
            }

            mylist = null;

            //reverse order


            string[] points = sb.ToString().Split(' ');
            StringBuilder revPoints = new StringBuilder();
            for (int i = points.Length - 3; i >= 0; i--)
            {
                if(points[i].Contains(",")){
                    revPoints.Append(points[i]);

                    if (i > 0)
                    {
                        revPoints.Append(" ");
                    }
                }
                
            }

            return polygonArea(revPoints.ToString());
        }


        /// <summary>
        /// get height of polygon from coordinates (y points)
        /// </summary>
        public static double polyHeight(string svgpoints)
        {

            string cleanPoints;
            cleanPoints = Common.cleanString(svgpoints);
            cleanPoints = cleanPoints.Replace("  ", " ");       

            string[] points = cleanPoints.Split(' ');            
            List<double> yp = new List<double>();

            //put the vertices in an array (just y)
            foreach (string point in points)
            {
                string[] xycoords = point.Split(',');
                if (xycoords.Count() > 1)
                {
                    yp.Add(Convert.ToDouble(xycoords[1]));
                }
            }

            double maxPoint = yp.Max();
            double minPoint = yp.Min();

            return convertUnitToMm(Math.Abs(maxPoint - minPoint));

        }


         /// <summary>
        /// get height of a path using polygon height method
        /// </summary>
        public double pathHeight(string myPath)
        {
            // just use sharpVector segmentList 
            SvgPathSegList mylist = new SvgPathSegList(myPath, true);
            StringBuilder sb = new StringBuilder();

            int segCount = 0;
            foreach (SvgPointF seg in mylist.Points)
            {
                sb.Append(Convert.ToString(seg.ValueX) + "," + Convert.ToString(seg.ValueY));
                segCount += 1;
                if (segCount < mylist.NumberOfItems)
                {
                    sb.Append(" ");
                }
            }

            mylist = null;

            string sbString = sb.ToString();
            sb = null;
            return polyHeight(sbString);

        }


        public static double convertUnitToMm(double unitValue)
        {
            //return unitValue * 0.2822222;
            return unitValue * 0.352777778;
        }

        public static double convertPixelToMm(double unitValue)
        {
            return unitValue * 0.2822222;
        }


        private static double inkPixelToMM(double unitValue)
        {
            double factor = 1.0 / 3.5433070866;

            return unitValue * factor;
        }


        /// <summary>
        /// Not using at this time
        /// </summary>
        /// <param name="myPath"></param>
        /// <returns></returns>
         public string parsePath(string myPath)
         {
             StringBuilder vertices = new StringBuilder();
             int tokencounter = 0;
             startPoint startPos = new startPoint(0, 0);

             startPoint currentPoint = new startPoint(0, 0);
             double pathPerimeter =0;
             string separators = @"(?=[A-Za-z])";
             var tokens = Regex.Split(myPath, separators).Where(t => !string.IsNullOrEmpty(t));


             // interpreter. Runs the list of commands.
             
             foreach (string token in tokens)
             {
                 // note that Parse could throw an exception
                 // if the path is not correct 
                 pathCommand c = pathCommand.Parse(token);

                 vertices.Append("\r\n" + c.command + "  ");

                 //calculate length based on command switch

                 switch (c.command)
                 {
                     case 'M':
                     case 'm':
                         // MoveTo = start point
                         vertices.Append("(");
                         int argCount = 0;
                         
                         if (tokencounter == 0)
                         {                             
                            startPos.x = c.arguments[0];
                            startPos.y = c.arguments[1];
                         }

                         //set the currentpoint
                         currentPoint.x = c.arguments[0];
                         currentPoint.y = c.arguments[1];

                         foreach (float arg in c.arguments)
                         {
                             if (argCount == c.argcount)
                             {
                                 
                          
                             }
                             vertices.Append(arg + " # ");
                             argCount += 1;
                         }
                         vertices.Append(")");
                        break;

                     case 'Z':
                     case 'z':
                        // close path straight line from current point to start point 
                        // calculate line length from currentPoint to startPoint

                        pathPerimeter += lineLength(currentPoint.x, startPos.x, currentPoint.y, startPos.y);

                        vertices.Append("(");
                        foreach (float arg in c.arguments)
                        {
                            vertices.Append(arg + " # ");
                        }
                        vertices.Append(")");
                        break;

                     case 'C':
                     case 'c':
                        // CurveTo a cubic Bezier curve from current point (x,y) http://www.w3.org/TR/SVG/paths.html#PathDataCubicBezierCommands
                        // x0/y0 and x3/y3 are the start and end points (this means x3/y3 will be set to the current point for the next iteration)
                        // x2/y2 and x4/y4 are the control points
                        vertices.Append("(");
                        foreach (float arg in c.arguments)
                        {
                            vertices.Append(arg + " # ");
                        }
                        vertices.Append(")");
                        break;

                     case 'S':
                     case 's':
                        // Shorthand cubic bezier curve, from the current point to (x, y), http://www.w3.org/TR/SVG/paths.html#PathDataCubicBezierCommands
                        vertices.Append("(");
                        foreach (float arg in c.arguments)
                        {
                            vertices.Append(arg + " # ");
                        }
                        vertices.Append(")");
                        break;

                     case 'Q':
                     case 'q':
                        //(x1 y1 x y)+
                        // quadratic bezier curve from the current point to (x,y) using (x1, y1) as the control point
                        vertices.Append("(");
                        foreach (float arg in c.arguments)
                        {
                            vertices.Append(arg + " # ");
                        }
                        vertices.Append(")");
                        break;

                     case 'T':
                     case 't':
                        //(x y)+
                        // shorthand/smooth quadratic bezier curve from the current point to (x,y) see w3 site!!
                        vertices.Append("(");
                        foreach (float arg in c.arguments)
                        {
                            vertices.Append(arg + " # ");
                        }
                        vertices.Append(")");
                        break;

                     case 'A':
                     case 'a':
                        //rx ry x-axis-rotation large-arc-flag sweep-flag x y)+
                        // draws an elliptical arc from the current point to (x,y)
                        vertices.Append("(");
                        foreach (float arg in c.arguments)
                        {
                            vertices.Append(arg + " # ");
                        }
                        vertices.Append(")");
                        break;

                     case 'L':
                     case 'l':
                        // lineto draw a line from the current point to the given (x,y) 

                        pathPerimeter += lineLength(c.arguments[0], currentPoint.x, c.arguments[1], currentPoint.y);

                        //set the currentpoint after using this arg
                        currentPoint.x = c.arguments[0];
                        currentPoint.y = c.arguments[1];

                        vertices.Append("(");
                        foreach (float arg in c.arguments)
                        {
                            vertices.Append(arg + " # ");
                        }
                        vertices.Append(")");
                        break;

                     case 'H':
                     case 'h':
                        // Horizontal line from current point (cpx, cpy) to (x, cpy)

                        pathPerimeter += lineLength(currentPoint.x, c.arguments[0], currentPoint.y, currentPoint.y);

                        //set the currentpoint after using this arg
                        currentPoint.x = c.arguments[0];
                        currentPoint.y = c.arguments[1];


                        vertices.Append("(");
                        foreach (float arg in c.arguments)
                        {
                            vertices.Append(arg + " # ");
                        }
                        vertices.Append(")");
                        break;

                     case 'V':
                     case 'v':
                        // lineto draw a vertical line from the current point to cpx, y

                        pathPerimeter += lineLength(currentPoint.x, currentPoint.x, currentPoint.y, c.arguments[1]);

                        //set the currentpoint after using this arg
                        currentPoint.x = c.arguments[0];
                        currentPoint.y = c.arguments[1];

                        vertices.Append("(");
                        foreach (float arg in c.arguments)
                        {
                            vertices.Append(arg + " # ");
                        }
                        vertices.Append(")");
                        break;
                     
                     
                                             

                     default:
                            // nothing?
                        break;
                 }



                 tokencounter += 1;
                 
                 
             }

             vertices.Append("\r\n line length: " + pathPerimeter);

             return vertices.ToString();
         }
        
    }
}
