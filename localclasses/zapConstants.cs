using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using SVGLib;
using System.Web.Configuration;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace priceSvg
{
    public class zapConstants
    {
        //python app and session calls        
        public static string pythonMethodPath;         
        public static ScriptRuntime ipy;
        public static dynamic pathCall;

        
        //const values to indicate type of cut/engrave
        public const int vectorCut = 1;
        public const int vectorLightEngrave = 2;
        public const int vectorMedEngrave = 3;
        public const int vectorHeavyEngrave = 4;        
        //fills
        public const int rasterLightEngrave = 5;
        public const int rasterMedEngrave = 6;
        public const int rasterHeavyEngrave = 7;
        // new raster line
        public const int rasterLine = 8;
        //new rast line shape
        public const int rasterLineShape = 9;



        //const values to specify colour of cut, engrave and fills
        public const string vectorCutColor = "25500";
        public const string vectorLightEngraveColor = "02550";
        public const string vectorMedEngraveColor = "00255";
        public const string vectorHeavyEngraveColor = "2550255";
        //fills
        public const string rasterLightEngraveColor = "230230230";
        public const string rasterMedEngraveColor = "128128128";
        public const string rasterHeavyEngraveColor = "000";
        public const string rasterIgnoreColour = "255255255";


        //line widths for cuts engraving in pt = 0.0283 mm = 0.01
        public const string vectorCutWidth = "0.0283";
        public const string vectorLightEngraveCutWidth = "0.0283";
        public const string vectorMediumEngraveCutWidth = "0.0283";
        public const string vectorHeavyEngraceCutWidth = "0.0283";
        // needs to be wider
        //public const double maxLineWidth = 0.0283;
        public const double maxLineWidth = 0.215;

        //new raster line - any line that is a raster colour over this width is treated as raster engrave area
        public const double rasterLineMinWidth = 2.16;


        // cutting times for each material per mm
        // not used values et in db for all times
        public const double acrylic3mmCut = 0.07;
        public const double acrylic5mmCut = 0.13;
        public const double acrylic10mmCut = 0.13;
        public const double plyWood3mmCut = 0.06;

        public const double globalEngraveTime = 0.04;
        
        //raster fill figures
        // 0.001 probably too high
        //public const double rasterEngravePerMM = 0.001;
        public const double rasterEngravePerMM = 0.0005;
        public const double rasterEngraveWidth = 0.05;
        public const double rasterTurnTime = 0.1045;

        //metrial types
        public const int mAcrylic3mm = 1;
        public const int mAcrylic5mm = 2;
        public const int mAcrylic10mm = 3;
        public const int mPlywood3mm = 4;


        //error types for cut, engrave and fill
        public const int errCutToThick = 1;
        public const int errCutContainsFill = 2;
        public const int errEngraveToThickBlue = 3;
        public const int errEngraveToThickGreen = 4;
        public const int errEngraveToThickMagenta = 5;
        public const int errEngraveFillBlue = 6;
        public const int errEngraveFillGreen = 7;
        public const int errEngraveFillMagenta = 8;
        public const int errColourMisMatch = 9;
        public const int errMiscStrokeErr = 10;
        public const int errUnknownFill = 11;
        public const int errContainsText = 12;



    }
}