using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SVGLib;

namespace priceSvg
{
    public class getElementData
    {

        public int zapType;
        public double length;
        public double area;
        public double rasterArea;
        public double xpos;
        public double ypos;
        public string userErrorMsg;        
        public string debugInfo;
        public bool userError = false;
        public string userErrorElement;
        public double shapeHeight;
        public List<Point> pointList = new List<Point>();

        public getElementData(SvgElement svge)
        {

            myShapes shapeData = new myShapes();
            errors errType = new errors();
                                    
            bool hasFill = svgFillColorFix.checkFill(svge.GetTagXml());
            
            
            switch (svge.getElementType())
            {
                case SvgElement._SvgElementType.typeCircle:
                        //create circle object & check circle errors
                        // check stroke color & width & fill to get zap type zapType = zapConstants.                    
                        circleData myCircle = new circleData((SvgCircle)svge);
                        
                        zapCompliance errCheck = new zapCompliance();

                        errCheck.checkStrokeFill(myCircle.strokeColor, myCircle.fillColor, myCircle.strokeWidth, hasFill);

                        if (errCheck.validElement)
                        {
                            length = myShapes.circleCircumference(myCircle.radius);
                            area = myShapes.circleArea(myCircle.radius);
                            xpos = myCircle.xpos;
                            ypos = myCircle.ypos;
                            zapType = errCheck.zapType;                            
                            shapeHeight = myShapes.convertPixelToMm(myShapes.circleHeight(myCircle.radius));
                                                        
                            // if raster line then change to rasterLineShape
                            if (isRasterLine(errCheck.zapType))
                            {
                                zapType = zapConstants.rasterLineShape;
                            }
                        }
                        else
                        {
                            //throw error of correct type to user and end
                            errType.getElementError(errCheck.zapErrorType);
                            userErrorMsg = errType.currentError;
                            userErrorElement = "Circle";
                            userError = true;
                        }

                        myCircle = null;
                        errCheck = null;
                    
                        break;

                case SvgElement._SvgElementType.typeLine:

                        lineData myLine = new lineData((SvgLine)svge);
                        zapCompliance lineCheck = new zapCompliance();

                        lineCheck.checkStrokeFill(myLine.strokeColor, myLine.fillColor, myLine.strokeWidth, hasFill);

                        if (lineCheck.validElement)
                        {
                            length = myShapes.lineLength(myLine.xpos1, myLine.xpos2, myLine.ypos1, myLine.ypos2);
                            //try distance calc
                            //double length2 = length = myShapes.distance(myLine.xpos1, myLine.xpos2, myLine.ypos1, myLine.ypos2);
                            area = 0;
                            xpos = myLine.xpos1;
                            ypos = myLine.ypos2;
                            zapType = lineCheck.zapType;
                            shapeHeight = myShapes.convertPixelToMm(Convert.ToDouble(myLine.strokeWidth));
                            // if raster line then add coordinates to points list
                            if (isRasterLine(lineCheck.zapType))
                            {                                
                                pointList.Add(new Point(myLine.xpos1, myLine.ypos1));                             
                            }
                        }
                        else
                        {
                            errType.getElementError(lineCheck.zapErrorType);
                            userErrorMsg = errType.currentError;
                            userErrorElement = "Line";
                            userError = true;
                        }

                        lineCheck = null;
                        myLine = null;
                    
                        break;

                case SvgElement._SvgElementType.typeEllipse:

                        ellipseData myEllipse = new ellipseData((SvgEllipse)svge);                        
                        zapCompliance ellipseCheck = new zapCompliance();

                        ellipseCheck.checkStrokeFill(myEllipse.strokeColor, myEllipse.fillColor, myEllipse.strokeWidth, hasFill);

                        if (ellipseCheck.validElement)
                        {
                            length = myShapes.ellipsePerimeter(myEllipse.rx, myEllipse.ry);
                            area = myShapes.ellipseArea(myEllipse.rx, myEllipse.ry);
                            xpos = myEllipse.cx;
                            ypos = myEllipse.cy;
                            zapType = ellipseCheck.zapType;
                            shapeHeight = myShapes.convertPixelToMm(myEllipse.ry);

                            // if raster line then change to rasterLineShape
                            if (isRasterLine(ellipseCheck.zapType))
                            {
                                zapType = zapConstants.rasterLineShape;
                            }
                        }
                        else
                        {
                            errType.getElementError(ellipseCheck.zapErrorType);
                            userErrorMsg = errType.currentError;
                            userErrorElement = "Ellipse";
                            userError = true;
                        }

                        myEllipse = null;
                        ellipseCheck = null;
                        break;

                case SvgElement._SvgElementType.typeRect:

                        rectangleData myRect = new rectangleData((SvgRect)svge);
                        zapCompliance rectCheck = new zapCompliance();

                        rectCheck.checkStrokeFill(myRect.strokeColor, myRect.fillColor, myRect.strokeWidth, hasFill);
                        
                        
                        if (rectCheck.validElement)
                        {
                            length = myShapes.rectanglePerimeter(myRect.rHeight, myRect.rWidth);
                            area = myShapes.rectangleArea(myRect.rHeight, myRect.rWidth);
                            xpos = myRect.xpos;
                            ypos = myRect.ypos;
                            zapType = rectCheck.zapType;
                            shapeHeight = myShapes.convertPixelToMm(myRect.rHeight);

                            // if raster line then change to rasterLineShape
                            if (isRasterLine(rectCheck.zapType))
                            {
                                zapType = zapConstants.rasterLineShape;
                            }
                        }
                        else
                        {
                            //throw error of correct type to user and end
                            errType.getElementError(rectCheck.zapErrorType);
                            userErrorMsg = errType.currentError;
                            userErrorElement = "Rectangle";
                            userError = true;
                        }

                        myRect = null;
                        rectCheck = null;
                        break;

                case SvgElement._SvgElementType.typePolygon:

                        polygonData myPolygon = new polygonData((SvgPolygon)svge);
                        zapCompliance polyCheck = new zapCompliance();

                        polyCheck.checkStrokeFill(myPolygon.strokeColor, myPolygon.fillColor, myPolygon.strokeWidth, hasFill);

                        if (polyCheck.validElement)
                        {                            
                            length = myShapes.polygonLength(myPolygon.points.Trim());
                            area = myShapes.polygonArea(myPolygon.points.Trim());
                            xpos = 0;
                            ypos = 0;
                            zapType = polyCheck.zapType;
                            shapeHeight = myShapes.polyHeight(myPolygon.points);

                            // if raster line then change to rasterLineShape
                            if (isRasterLine(polyCheck.zapType))
                            {
                                zapType = zapConstants.rasterLineShape;
                            }
                        }
                        else
                        {
                            errType.getElementError(polyCheck.zapErrorType);
                            userErrorMsg = errType.currentError;
                            userErrorElement = "Polygon";
                            userError = true;
                        }

                        myPolygon = null;
                        polyCheck = null;

                        break;

                case SvgElement._SvgElementType.typePolyLine:

                        polylineData myPolyLine = new polylineData((SvgPolyLine)svge);
                        zapCompliance polylineCheck = new zapCompliance();

                        polylineCheck.checkStrokeFill(myPolyLine.strokeColor, myPolyLine.fillColor, myPolyLine.strokeWidth, hasFill);

                        if (polylineCheck.validElement)
                        {
                            length = myShapes.sqlpolyLineLength(myPolyLine.points.Trim());
                            area = 0;
                            xpos = 0;
                            ypos = 0;
                            zapType = polylineCheck.zapType;
                            shapeHeight = 0;

                            // if raster line then change to rasterLineShape
                            if (isRasterLine(polylineCheck.zapType))
                            {
                                zapType = zapConstants.rasterLineShape;
                            }
                        }
                        else
                        {
                            errType.getElementError(polylineCheck.zapErrorType);
                            userErrorMsg = errType.currentError;
                            userErrorElement = "Polyline";
                            userError = true;
                        }

                        myPolyLine = null;
                        polylineCheck = null;

                        break;

                case SvgElement._SvgElementType.typePath:

                        pathData myPath = new pathData((SvgPath)svge);
                        zapCompliance pathCheck = new zapCompliance();
                                            

                        pathCheck.checkStrokeFill(myPath.strokeColor, myPath.fillColor, myPath.strokeWidth, hasFill);

                        if (pathCheck.validElement)
                        {
                            string pathData = myPath.shapeData;

                            length = myShapes.getPathLength(pathData);
                            //if zaptype is a fill calculate the area - save processing when not required
                            if (pathCheck.validFill)
                            {
                                area = myShapes.getPathAreaInk(pathData);
                            }
                            else
                            {
                                area = 0;
                            }
                            xpos = 0;
                            ypos = 0;
                            zapType = pathCheck.zapType;                                                        
                            shapeHeight = myShapes.convertPixelToMm(shapeData.pathHeight(pathData));

                            // if raster line then change to rasterLineShape
                            if (isRasterLine(pathCheck.zapType))
                            {                                
                                zapType = zapConstants.rasterLineShape;
                            }
                        }
                        else
                        {
                            errType.getElementError(pathCheck.zapErrorType);
                            userErrorMsg = errType.currentError;
                            userErrorElement = "Path (stroke colour: " + myPath.strokeColor + " stroke width: " + myPath.strokeWidth + " fill colour: " + myPath.fillColor + " ID: " + myPath.Id + ")" ;
                            userError = true;
                        }

                        myPath = null;
                        pathCheck = null;

                        break;

                default:
                    //svglist = "blank";
                    // unknown element return error of correct type

                    //check if text or image 
                        switch (svge.getElementType())
                        {
                            case SvgElement._SvgElementType.typeImage:

                                errType.errBitmap();
                                userErrorMsg = errType.currentError ;
                                userErrorElement = "Image";
                                userError = true;
                                break;

                            case SvgElement._SvgElementType.typeText:
                                errType.getElementError(zapConstants.errContainsText);
                                userErrorMsg = errType.currentError;
                                userErrorElement = "Text";
                                userError = true;
                                break;

                            default:
                                userErrorMsg = errType.currentError;
                                userErrorElement = "Unknown Element in Design Error";
                                userError = true;
                                break;
                        }
                    
                        zapType = 0;
                    length = 0;
                    area = 0;
                    xpos = 0;
                    ypos = 0;
                    
                    break;
            }

            shapeData = null;

        }


        public static bool isRasterLine(int z) {
            
            return z == zapConstants.rasterLine;
        }


    }


}